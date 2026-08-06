using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLife.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId {  get; set; }
        public Category? Category { get; set; }
    }
}
