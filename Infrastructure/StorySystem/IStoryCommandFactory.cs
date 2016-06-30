using System;
using System.Collections.Generic;
namespace StorySystem
{
    public interface IStoryCommandFactory
    {
        IStoryCommand Create();
    }
    public class StoryCommandFactoryHelper<T> : IStoryCommandFactory where T : IStoryCommand, new()
    {
        public IStoryCommand Create()
        {
            T t = new T();
            return t;
        }
    }
}
