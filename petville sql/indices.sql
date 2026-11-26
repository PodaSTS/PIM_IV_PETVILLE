
-- INDICES PARA OTIMIZACAO DO BANCO PETVILLE

CREATE INDEX idx_proprietario_nome
ON proprietario(nome);

CREATE INDEX idx_animal_nome
ON animal(nome);

CREATE INDEX idx_produto_nome
ON produto(nome);

CREATE INDEX idx_funcionario_nome
ON funcionario(nome);

CREATE INDEX idx_veterinario_nome
ON veterinario(nome);

CREATE INDEX idx_fornecedor_nome
ON fornecedor(nome);

CREATE INDEX idx_compra_data
ON compra(data_compra);

CREATE INDEX idx_consulta_data
ON consulta(data_consulta);

CREATE INDEX idx_solicitacao_data
ON solicitacao(data_solicitacao);
