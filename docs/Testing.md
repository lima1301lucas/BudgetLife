# Testes Manuais — BudgetLife

Testes end-to-end realizados manualmente via console, validando o fluxo completo da aplicação (Model → Repository → Service → UI) com dados reais no banco MySQL.

## Categorias

- [x] Listar categorias existentes (seed) — as 8 categorias iniciais (Food, Transportation, Housing, Leisure, Health, Education, Credit Card, Others) apareceram corretamente
- [x] Cadastrar nova categoria — categorias "Pets" e "Streaming" cadastradas e confirmadas na listagem (10 categorias no total)
- [x] Editar categoria — alteração de nome refletida corretamente na listagem
- [x] Excluir categoria sem despesas vinculadas — OK
- [x] Excluir categoria **com** despesas vinculadas — bloqueado corretamente pela validação `ExistsByCategoryId`, com a mensagem: *"Não é possível excluir: existem despesas vinculadas a essa categoria."*

## Despesas

- [x] Cadastrar despesas em categorias e datas diferentes — 6 despesas cadastradas, variando categoria e mês
- [x] Listar despesas — nome da categoria exibido corretamente ao lado de cada despesa (confirma o `JOIN` entre `expenses` e `categories`)
- [x] Editar despesa — valor alterado (Amazon: R$ 10 → R$ 15) e refletido corretamente na listagem
- [x] Excluir despesa (com confirmação S/N) — exclusão da despesa "shopee" confirmada e removida da listagem

## Relatórios

Valores conferidos manualmente com base nas despesas cadastradas.

- [x] **Total por categoria** — Education: R$ 65,00 | Housing: R$ 550,50 | Leisure: R$ 1.000,00 | Others: R$ 275,00 — todos batendo com a soma esperada
- [x] **Total por mês** (ano 2026) — Agosto: R$ 1.015,00 | Julho: R$ 295,00 | Maio: R$ 30,00 | Setembro: R$ 550,50 — todos batendo com a soma esperada
- [x] **Despesas por período** (01/01/2026 a 31/12/2026) — retornou todas as despesas do intervalo, com categoria correta em cada uma

## Validações e tratamento de erro

- [x] Nome vazio — rejeitado, loop pede novamente
- [x] Valor não numérico ("abc", "acb1") — rejeitado, loop pede novamente até valor válido
- [x] Data com formato incompleto ("11/11", sem ano) — inicialmente aceita por engano (`DateTime.TryParse` era flexível demais); **corrigido** para `DateTime.TryParseExact` exigindo o formato `dd/MM/yyyy`
- [x] Data com separador errado ("11-11-2011") — rejeitada corretamente após a correção acima
- [x] Data estruturalmente inválida ("32/13/2026") — rejeitada corretamente
- [x] Id de categoria inexistente (999) — rejeitado com mensagem clara: *"Categoria não encontrada."*
- [x] Erro de conexão com o banco — testado isoladamente no `Program.cs`: se a conexão falhar antes de iniciar o menu, exibe mensagem amigável e encerra o programa sem stack trace técnica
- [x] Exceções inesperadas — capturadas por um `catch (Exception ex)` genérico em todas as operações de escrita do menu, como rede de segurança além das validações de negócio (`ArgumentException`)

## Observações

- Todas as regras de negócio (nome obrigatório, valor > 0, categoria existente, categoria com despesas vinculadas) foram validadas tanto pela camada de `Service` quanto reforçadas pela UI, seguindo o princípio de defesa em profundidade.
- Os testes foram realizados manualmente, sem uso de testes automatizados (fora do escopo deste projeto).