# ProjetoTransacaoFinanceira
Transações Financeiras

Este projeto é uma aplicação em C# que simula 8 transações financeiras entre diferentes contas bancárias. 

Objetivo

O principal objetivo deste projeto é corrigir um sistema de transferências de valores entre contas bancárias, garantindo que apenas transações válidas sejam executadas, ou seja, que as contas de origem tenham saldo suficiente e que ambas as contas existam no sistema.

Funcionalidades
    Cadastro de Contas: O sistema mantém um repositório de contas bancárias, cada uma com um saldo inicial.
    Transferências: Realiza transações entre contas, com validações de saldo e existência das contas.
    Exibição de Saldos: Após a execução das transações, o saldo final de cada conta é exibido.
    Tratamento de Erros: Verifica se a conta de origem possui saldo suficiente e se as contas envolvidas nas transações existem.
    Execução Ordenada: As transações são executadas na ordem cronológica, conforme a data e hora especificadas.

Estrutura do Projeto
    Program.cs: O ponto de entrada do programa, onde as transações são carregadas, ordenadas e executadas.
    Transacao.cs: Representa uma transação financeira, contendo informações como ID, data/hora, contas envolvidas, e valor.
    IRepositorioContas.cs: Interface para o repositório de contas, com métodos para buscar e atualizar contas.
    RepositorioContas.cs: Implementação do repositório de contas utilizando um ConcurrentDictionary para armazenar as contas.
    Conta.cs: Classe que representa uma conta bancária, com métodos para debitar e creditar valores.
    ExecutorTransacao.cs: Classe responsável por executar as transações, realizando as devidas validações e atualizações nas contas.

Boas Práticas Aplicadas
    Princípios SOLID: A separação de responsabilidades e o uso de interfaces garantem um design modular e extensível.
    Uso de Moq para Testes Unitários: Utilização do framework Moq para criar mocks do repositório e testar os diferentes cenários de transações.
    Testes Unitários: Implementação de testes unitários para garantir que as transações sejam executadas corretamente, incluindo cenários de sucesso e falha.

Como Executar
    Clone o repositório.
    Abra o projeto em sua IDE preferida.
    Compile e execute a aplicação para ver o resultado das transações financeiras.
    Execute os testes unitários para garantir a integridade do código.

Testes Unitários
    Este projeto inclui uma suíte de testes unitários para verificar o correto funcionamento do sistema de transações financeiras. Os testes foram desenvolvidos utilizando NUnit e Moq para garantir a cobertura de diferentes cenários no processo de transferência de valores entre contas bancárias.
Estrutura dos Testes
    Os testes estão organizados na classe ExecutorTransacaoTests, que valida o comportamento da classe ExecutorTransacao, responsável por realizar as transferências.


Tecnologias Utilizadas
    NUnit
    Moq
    
