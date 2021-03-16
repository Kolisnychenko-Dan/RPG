
using System.Collections.Generic;
using RPG.Stats;

public interface IAdditiveModifier
{
    IEnumerable<float> GetAdditiveModifier(Stat stat);
}