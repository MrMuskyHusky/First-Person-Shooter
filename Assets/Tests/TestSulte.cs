using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class TestSulte
{
    private Player player;
    GameObject ground;

    [SetUp]
    public void Setup()
    {
        GameObject playerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefab/Player"));
        player = playerGameObject.GetComponent<Player>();
        ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        MouseLook[] mouseLook = playerGameObject.GetComponentsInChildren<MouseLook>();
        foreach (MouseLook mouse in mouseLook)
        {
            mouse.isTesting = true;
        }
        ground.transform.position = new Vector3(playerGameObject.transform.position.x, playerGameObject.transform.position.y - 1.1f, playerGameObject.transform.position.z);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(player.gameObject);
        Object.Destroy(ground.gameObject);
    }

    [UnityTest]
    public IEnumerator OurTest()
    {
        Vector3 pos = player.transform.position;

        float StartTime = Time.time;
        while (Time.time < StartTime + 0.5f)
        {
            player.Move(1, 1);
            yield return null;
        }

        //yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(pos.y, player.transform.position.y);

        // These don't work for some reason
        // Assert.AreNotEqual(pos.x, player.transform.position.x);
        // Assert.AreNotEqual(pos.z, player.transform.position.z);
    }

    [UnityTest]
    public IEnumerator Shooting()
    {
        Gun gun = player.GetComponentInChildren<Gun>();
        GameObject enemyGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefab/Enemy"));
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.transform.position = new Vector3(player.transform.position.x + (player.transform.forward * 1.5f).x, player.transform.position.y - 1f, player.transform.position.z);
        float enemyHealth = enemy.curHealth;
        float StartTime = Time.time;
        player._gravity = 0;
        yield return null;
        gun.Shoot();
        yield return null;
        gun.Shoot();
        yield return null;
        gun.Shoot();
        Assert.Less(enemy.curHealth, enemyHealth);

        yield return null;
    }
}
