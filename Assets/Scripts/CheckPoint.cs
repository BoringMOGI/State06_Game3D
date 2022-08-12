using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if(player != null)
        {
            player.SetRespawn(respawnPoint);
            Destroy(this);                          // 해당 컴포넌트를 삭제한다.
            //enabled = false;                      // 체크 포인트 후에는 컴포넌트를 끈다.
        }
    }
}
