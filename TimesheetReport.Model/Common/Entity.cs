using SingLife.PriceComparison.Model.Common;
using System;

namespace TimesheetReport.Model.Common
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        protected void InitializeId()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }

        protected void SetId(Guid id)
        {
            Id = id;
        }
    }
}