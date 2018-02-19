using Unity.Jobs;

namespace DanmakU {

public interface IDanmakuModifier {

  JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency);

}

}