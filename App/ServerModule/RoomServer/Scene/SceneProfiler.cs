using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
  internal sealed class SceneProfiler
  {
    internal long DelayActionProcessorTime = 0;
    internal long MovementSystemTime = 0;
    internal long AiSystemTime = 0;
    internal long SceneLogicSystemTime = 0;
    internal long StorySystemTime = 0;
    internal long TickSkillTime = 0;
    internal long TickEntitiesTime = 0;
    internal long TickLevelupTime = 0;
    internal long TickAttrRecoverTime = 0;
    internal long TickDebugSpaceInfoTime = 0;

    internal string GenerateLogString(int sceneId,long elapsedTime)
    {
      StringBuilder builder = new StringBuilder();

      builder.Append("=>SceneResourceId:").Append(sceneId).Append(",ElapsedTime:").Append(elapsedTime).AppendLine();

      builder.Append("=>DelayActionProcessorTime:").Append(DelayActionProcessorTime).AppendLine();
      
      builder.Append("=>MovementSystemTime:").Append(MovementSystemTime).AppendLine();

      builder.Append("=>AiSystemTime:").Append(AiSystemTime).AppendLine();

      builder.Append("=>SceneLogicSystemTime:").Append(SceneLogicSystemTime).AppendLine();

      builder.Append("=>StorySystemTime:").Append(StorySystemTime).AppendLine();
      
      builder.Append("=>TickSkillTime:").Append(TickSkillTime).AppendLine();
      
      builder.Append("=>TickNpcsTime:").Append(TickEntitiesTime).AppendLine();
      
      builder.Append("=>TickLevelupTime:").Append(TickLevelupTime).AppendLine();
      
      builder.Append("=>TickAttrRecoverTime:").Append(TickAttrRecoverTime).AppendLine();

      builder.Append("=>TickDebugSpaceInfoTime:").Append(TickDebugSpaceInfoTime).AppendLine();

      return builder.ToString();
    }
  }
}
