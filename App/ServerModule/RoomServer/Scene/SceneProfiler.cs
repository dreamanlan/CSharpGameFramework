using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
  public sealed class SceneProfiler
  {
    public long DelayActionProcessorTime = 0;
    public long MovementSystemTime = 0;
    public long AiSystemTime = 0;
    public long SceneLogicSystemTime = 0;
    public long StorySystemTime = 0;
    public long TickEntitiesTime = 0;
    public long TickLevelupTime = 0;
    public long TickAttrRecoverTime = 0;
    public long TickDebugSpaceInfoTime = 0;

    public string GenerateLogString(int sceneId,long elapsedTime)
    {
      StringBuilder builder = new StringBuilder();

      builder.Append("=>SceneResourceId:").Append(sceneId).Append(",ElapsedTime:").Append(elapsedTime).AppendLine();

      builder.Append("=>DelayActionProcessorTime:").Append(DelayActionProcessorTime).AppendLine();
      
      builder.Append("=>MovementSystemTime:").Append(MovementSystemTime).AppendLine();

      builder.Append("=>AiSystemTime:").Append(AiSystemTime).AppendLine();

      builder.Append("=>SceneLogicSystemTime:").Append(SceneLogicSystemTime).AppendLine();

      builder.Append("=>StorySystemTime:").Append(StorySystemTime).AppendLine();
      
      builder.Append("=>TickEntitiesTime:").Append(TickEntitiesTime).AppendLine();
      
      builder.Append("=>TickLevelupTime:").Append(TickLevelupTime).AppendLine();
      
      builder.Append("=>TickAttrRecoverTime:").Append(TickAttrRecoverTime).AppendLine();

      builder.Append("=>TickDebugSpaceInfoTime:").Append(TickDebugSpaceInfoTime).AppendLine();

      return builder.ToString();
    }
  }
}
