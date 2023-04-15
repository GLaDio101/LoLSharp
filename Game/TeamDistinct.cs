using System.Linq;
using LoLSharp.Game.Objects;
using LoLSharp.Modules;

namespace LoLSharp.Game
{
  public class TeamDistinct
  {
    private readonly EntityVo[] _entities;
    private readonly EntityVo[] _allies;
    private readonly EntityVo[] _enemies;

    private int _team;

    public TeamDistinct(EntityVo[] entities)
    {
      _team = LocalPlayer.Vo.GetTeam();

      _entities = entities;
      _allies = _entities.Where(vo => vo.Team == _team).ToArray();
      _enemies = _entities.Where(vo => vo.Team != _team).ToArray();
    }

    public EntityVo[] GetAllies()
    {
      return _allies;
    }

    public EntityVo[] GetEnemies()
    {
      return _enemies;
    }
  }
}
