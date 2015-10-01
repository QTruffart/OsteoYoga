using System;

namespace OsteoYoga.Domain.Models
{
    public class DefaultWorkDay : Entity
    {
        public virtual string DayOfTheWeek { get; set; }

        public virtual DayOfWeek DayOfWeek()
        {
            DayOfWeek dayOfWeek;
            if (Enum.TryParse(DayOfTheWeek, out dayOfWeek))
            {
                return dayOfWeek;
            }
            throw new Exception($"Impossible de convertir en DayOfWeek : {DayOfTheWeek}");
        }
    }
}
