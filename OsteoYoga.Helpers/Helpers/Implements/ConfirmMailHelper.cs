using OsteoYoga.Helper.Helpers.Abstracts;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class ConfirmMailHelper : BaseMailHelper
    {
        public override string TemplatePath => "OsteoYoga.Helper.FileResources.Template.AccountConfirmTemplate.cshtml";
        public override string Subject => "Ostéoyoga - Code d'activation";
    }
}
