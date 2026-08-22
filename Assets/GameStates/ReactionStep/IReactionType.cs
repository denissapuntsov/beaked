using System;

public interface IReactionType
{
    public Action<IReactionType> OnComplete { get; set; }
}
