
DELIMITER //

CREATE TRIGGER validar_estoque_item_compra
BEFORE INSERT ON item_compra
FOR EACH ROW
BEGIN
    DECLARE v_estoque INT;

    IF NEW.quantidade IS NULL OR NEW.quantidade <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Quantidade invalida';
    END IF;

    SELECT quantidade
    INTO v_estoque
    FROM produto
    WHERE cod = NEW.cod_produto;

    IF v_estoque IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Produto inexistente';
    END IF;

    IF v_estoque < NEW.quantidade THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Estoque insuficiente';
    END IF;
END //

DELIMITER ;
