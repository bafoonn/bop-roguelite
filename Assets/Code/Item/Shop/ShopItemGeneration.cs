using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Pasta
{
    public class ShopItemGeneration : MonoBehaviour
    {
        [SerializeField]
        private ItemPickup pickupPrefab = null;
        private GameObject item;
        //private bool isActive = false;
        //private float timer = 0.5f;
        private Collider2D itemCollider;
        private string cost;
        private Text costDisplay;

        // Start is called before the first frame update
        void Start()
        {

        }
        private void Update()
        {
            //if (timer <= 0)
            //{
            //    isActive = true;
            //    itemCollider.isTrigger = true;
            //}

            //else
            //{
            //    timer -= Time.deltaTime;
            //}
        }
        //private void OnTriggerExit2D(Collider2D col)
        //{
        //    if (col.gameObject.CompareTag("Player") && !isActive)
        //    {
        //        isActive = true;
        //        itemCollider.isTrigger = true;
        //    }
        //}
        public void ItemBought()
        {
            costDisplay.text = "";
        }
        public void GenerateItem()
        {
            if (item != null)
            {
                Destroy(item);
            }

            var rewards = Items.Current.GetRewards();
            var pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            int random = Random.Range(0, rewards.Count);
            pickup.Setup(rewards[random], true);
            pickup.transform.SetParent(this.gameObject.transform);
            cost = pickup.Item.cost.ToString();

            item = pickup.gameObject;
            itemCollider = item.GetComponent<Collider2D>();
            itemCollider.isTrigger = true;

            costDisplay = GetComponentInChildren<Text>();
            costDisplay.text = cost;
        }
    }
}
