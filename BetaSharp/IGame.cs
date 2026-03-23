using BetaSharp.Entities;
using BetaSharp.Worlds;

namespace BetaSharp;

public interface IGame
{
    World world { get; }
    EntityPlayer player { get; }
    bool isExternalMultiplayer { get; }
}