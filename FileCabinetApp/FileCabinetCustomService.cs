using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    class FileCabinetCustomService : FileCabinetService
    {
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
