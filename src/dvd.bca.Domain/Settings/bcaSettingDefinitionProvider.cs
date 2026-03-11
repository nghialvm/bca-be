using Volo.Abp.Settings;

namespace dvd.bca.Settings;

public class bcaSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(bcaSettings.MySetting1));
    }
}
