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
        /// Topic name in pose msg.
        /// </summary>
        public string stateTopic = "/traffic_light_data";

        /// <summary>
        /// Traffic light frame id.
        /// </summary>
        public string frameId = "traffic_light";

        IPublisher<std_msgs.msg.String> trafficLightPublisher;
        TrafficLight trafficlight;

        void Start()
        {
            trafficlight = GetComponent<TrafficLight>();
        }

        void Update()
        {
            trafficLightPublisher = SimulatorROS2Node.CreatePublisher<std_msgs.msg.String>(stateTopic);
            std_msgs.msg.String msg = new std_msgs.msg.String();
            AWSIM.TrafficLight.BulbData[] bulbDataArray = trafficlight.GetBulbData();
                
            msg.Data = trafficlight.UniqueId.ToString() + " ";

            for (int i = 0; i < bulbDataArray.Length; i++)
            {
                msg.Data += bulbDataArray[i].Color.ToString() + " ";
            }

            trafficLightPublisher.Publish(msg);
        }
    }

}  // namespace AWSiIM
