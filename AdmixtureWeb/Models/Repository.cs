using System.Collections.Generic;
using System.Linq;

namespace AdmixtureWeb.Models
{
    public class Repository
    {
        private readonly Calculator[] calculators;

        public Repository()
        {
            calculators =
               new[]
               {
                    new Calculator { Id = 1, Name = "Eurogenes K13", FileName = "E13.csv" },
                    new Calculator { Id = 2, Name = "Eurogenes K15", FileName = "E15.csv" },
                    new Calculator { Id = 3, Name = "Eurogenes K36", FileName = "E36.csv" },
                    new Calculator { Id = 4, Name = "MLDP K16", FileName = "MLDP16.csv" },
                    new Calculator { Id = 5, Name = "Custom K45", FileName = "K45.csv" }
               };

        }

        public Calculator GetCalculator(int id)
        {
            return calculators.SingleOrDefault(x => x.Id == id);
        }

        public IReadOnlyCollection<Calculator> GetCalculators()
        {
            return calculators;
        }
    }
}
