using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.BLL.Helpers
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression, int syncInterval = 0)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            SyncInterval = syncInterval;

        }

        public Type JobType { get; }
        public string CronExpression { get; }
        public int SyncInterval { get; set; }
    }
}
