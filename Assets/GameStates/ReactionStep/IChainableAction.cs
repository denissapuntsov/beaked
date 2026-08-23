using System;
using System.Collections.Generic;
using System.Linq;

public interface IChainableAction
{
    public Action<IChainableAction> OnComplete { get; set; }
    public void AddReaction(IChainableAction reaction);
}
