using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using System.Timers;
using ROS2;

namespace AWSIM
{

    /// <summary>
    /// Send traffic light bulb state via ROS2 communication
    /// </summary>
    public class TrafficLightPublisher : MonoBehaviour
    {

        /// <summary>
        /// Topic name of Traffic Light State msg.
        /// </summary>
        public string stateTopic = "/traffic_light_data";

        /// <summary>
        /// Traffic light frame id.
        /// </summary>
        public string frameId = "traffic_light";

        IPublisher<std_msgs.msg.String> trafficLightPublisher;
        TrafficLight trafficlight;
        float timer = 0;

        void Start()
        {
            trafficlight = GetComponent<TrafficLight>();
        }

        void Update()
        {
            // Update every second
            timer += Time.deltaTime;
            var interval = 1.0f;        // 1 second
            interval -= 0.00001f;       // Allow for accuracy errors.
            if (timer < interval)
                return;
            timer = 0;

            trafficLightPublisher = SimulatorROS2Node.CreatePublisher<std_msgs.msg.String>(stateTopic);
            std_msgs.msg.String msg = new std_msgs.msg.String();
            AWSIM.TrafficLight.BulbData[] bulbDataArray = trafficlight.GetBulbData();
                
            msg.Data = trafficlight.UniqueId.ToString() + " "; 
        
            //add the global coordinates
            //Since the local referential is rotated 90 degrees, we need to turn it

            Vector3 globalPosition = trafficlight.transform.position;
            globalPosition.z = Environment.Instance.MgrsOffsetPosition.z + globalPosition.y;
            globalPosition.y = Environment.Instance.MgrsOffsetPosition.y + globalPosition.x;
            globalPosition.x = Environment.Instance.MgrsOffsetPosition.x + globalPosition.z;

            //sending the global position of the traffic light        
            msg.Data += globalPosition.ToString() + " ";

            //sending the state of the traffic light
            for (int i = 0; i < bulbDataArray.Length; i++)
            {
                msg.Data += bulbDataArray[i].Type.ToString() + " ";
                msg.Data += bulbDataArray[i].Status.ToString() + " ";
            }

            trafficLightPublisher.Publish(msg);
        }
    }

}  // namespace AWSiIM
