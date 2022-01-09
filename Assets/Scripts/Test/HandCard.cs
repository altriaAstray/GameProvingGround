using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class HandCard : MonoBehaviour
    {
        [SerializeField] float height = 0.169f;
        [SerializeField] float spacing = -3.80f;
        [SerializeField] float bentAngle = 27f;
        [SerializeField] float cardWidth = 2f;

        [SerializeField] float rotationSpeed = 2f;
        [SerializeField] float rotationSpeedP2 = 346.1496f;
        [SerializeField] float movementSpeed = 4.4f;

        [SerializeField] GameObject CardPrefab;
        [SerializeField] Transform pivot;

        List<GameObject> cards = new List<GameObject>();

        void Start()
        {
            var offsetZ = -1;
            var layerZ = 0;
            for (int i = 0; i < 2; i++) 
            {
                GameObject go = Instantiate(CardPrefab, transform.position, Quaternion.identity);
                go.transform.SetParent(transform);

                var localCardPosition = go.transform.localPosition;
                localCardPosition.z = layerZ;
                go.transform.localPosition = localCardPosition;

                cards.Add(go);

                layerZ += offsetZ;
            }

           
        }

        // Update is called once per frame
        void Update()
        {
            Ben();
        }

   
        void Ben()
        {
            float fullAngle = -bentAngle;
            float anglePerCard = fullAngle / cards.Count;
            var firstAngle = CalcFirstAngle(fullAngle);
            var handWidth = CalcHandWidth(cards.Count);

            var pivotLocationFactor = 0;//pivot.CloserEdge(Camera.main, Screen.width, Screen.height);

            //计算X轴上偏移的第一个位置
            var offsetX = pivot.position.x - handWidth / 2;

            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];

                //set card Z angle
                var angleTwist = (firstAngle + i * anglePerCard) * pivotLocationFactor;

                //calc x position
                var xPos = offsetX + cardWidth / 2;

                //calc y position
                var yDistance = Mathf.Abs(angleTwist) * height;
                var yPos = pivot.position.y - yDistance * pivotLocationFactor;

                ////set position
                //if (!card.IsDragging && !card.IsHovering)
                //{
                var zAxisRot = pivotLocationFactor == 1 ? 0 : 180;
                var rotation = new Vector3(0, 0, angleTwist - zAxisRot);
                var position = new Vector3(xPos, yPos, card.transform.position.z);

                var rotSpeed = rotationSpeed;//card.IsPlayer ? parameters.RotationSpeed : parameters.RotationSpeedP2;

                card.transform.eulerAngles = rotation;

                var target = position;
                target.z = card.transform.position.z;
                card.transform.position = target;

                //}

                //increment offset
                offsetX += cardWidth + spacing;
            }
        }


        /// <summary>
        ///     Calculus of the angle of the first card.
        ///     第一张牌的角度的微积分。
        /// </summary>
        /// <param name="fullAngle">全角</param>
        /// <returns></returns>
        static float CalcFirstAngle(float fullAngle)
        {
            //神奇的数学因子
            float magicMathFactor = 0.1f;
            float result = -(fullAngle / 2) + fullAngle * magicMathFactor;
            Debug.Log(result);
            return result;
        }

        /// <summary>
        ///     Calculus of the width of the player's hand.
        ///     计算玩家手的宽度。
        /// </summary>
        /// <param name="quantityOfCards"></param>
        /// <returns></returns>
        float CalcHandWidth(int quantityOfCards)
        {
            var widthCards = quantityOfCards * cardWidth;
            var widthSpacing = (quantityOfCards - 1) * spacing;
            return widthCards + widthSpacing;
        }
    }
}