
DELIMITER //

CREATE PROCEDURE registrar_compra(
    IN p_cod_proprietario INT,
    IN p_cod_funcionario INT,
    IN p_data_compra DATE,
    IN p_forma_pagamento VARCHAR(50),
    IN p_nota VARCHAR(100),
    IN p_cod_produto INT,
    IN p_quantidade INT
)
BEGIN
    DECLARE v_estoque INT DEFAULT NULL;
    DECLARE v_valor DECIMAL(10,2);
    DECLARE v_cod_compra INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF p_quantidade IS NULL OR p_quantidade <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Quantidade invalida';
    END IF;

    START TRANSACTION;

    SELECT quantidade, valor
    INTO v_estoque, v_valor
    FROM produto
    WHERE cod = p_cod_produto
    FOR UPDATE;

    IF v_estoque IS NULL OR v_valor IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Produto inexistente ou dados incompletos';
    END IF;

    IF v_estoque < p_quantidade THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Estoque insuficiente';
    END IF;

    INSERT INTO compra (
        cod_proprietario,
        cod_funcionario,
        data_compra,
        valor,
        forma_pagamento,
        nota
    )
    VALUES (
        p_cod_proprietario,
        p_cod_funcionario,
        p_data_compra,
        v_valor * p_quantidade,
        p_forma_pagamento,
        p_nota
    );

    SET v_cod_compra = LAST_INSERT_ID();

    INSERT INTO item_compra (
        cod_compra,
        cod_produto,
        quantidade,
        valor_unitario
    )
    VALUES (
        v_cod_compra,
        p_cod_produto,
        p_quantidade,
        v_valor
    );

    UPDATE produto
    SET quantidade = quantidade - p_quantidade
    WHERE cod = p_cod_produto;

    COMMIT;
END //

DELIMITER ;
