using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace IsoRush.Level
{
    public class Checkpoint : MonoBehaviour
    {
        public async Task Spawn()
        {
            transform.position = new Vector3(0, 10, 0);
            await transform.DOLocalMoveY(0, 1).AsyncWaitForCompletion();
        }

        public async Task Despawn()
        {
            await transform.DOLocalMoveY(10, 1).AsyncWaitForCompletion();

            Destroy(this);
        }
    }
}
