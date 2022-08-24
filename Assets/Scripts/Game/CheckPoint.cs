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
            Destroy(this);                          // �ش� ������Ʈ�� �����Ѵ�.
            //enabled = false;                      // üũ ����Ʈ �Ŀ��� ������Ʈ�� ����.
        }
    }
}
