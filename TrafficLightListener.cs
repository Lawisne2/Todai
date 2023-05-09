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

    public class TrafficLightListner : MonoBehaviour
    {
        ISubscription<std_msgs.msg.String> trafficLightSubscriber;

        private void TrafficLightCallback(std_msgs.msg.String msg)
        {
            Debug.Log("Unity listener heard: [" + msg.Data + "]");
        }

        // Update is called once per frame
        void Update()
        {
            std_msgs.msg.String msg = new std_msgs.msg.String();
            trafficLightSubscriber = SimulatorROS2Node.CreateSubscription<std_msgs.msg.String>("/traffic_light_data", TrafficLightCallback);
        }
    }
}
