using UnityEngine;
using System.Collections;
using Vexe.Runtime.Types;

namespace Hourai {

    [DefineCategories("Tags")]
    public abstract class GameConfig : BaseScriptableObject {
        
        [Serialize, Show, Tags, Category("Tags"), Default("Player")]
        private string _playerTag;

        public string PlayerTag {
            get { return _playerTag; }
        }

        [Serialize, Show, Tags, Category("Tags"), Default("Respawn")]
        private string _respawnTag;

        public string RespawnTag {
            get { return _respawnTag; }
        }

        [Serialize, Show, Tags, Category("Tags"), Default("GUI")]
        private string _guiTag;

        public string GUITag {
            get { return _guiTag; }
        }

    }

}

