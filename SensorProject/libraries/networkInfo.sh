#!/bin/bash
echo "\"WlanMAC\":\""
ifconfig wlan0 | grep 'HWaddr' | cut -d: -f2,3,4,5,6,7 | awk '{echo $3}'
echo "\",\"WlanIP\":\""
ifconfig wlan0 | grep 'inet addr:' | cut -d: -f2 | awk '{ echo $1}'
echo "\",\"WlanSubnet\":\""
ifconfig wlan0 | grep 'inet addr:' | cut -d: -f4
echo "\",\"EthernetMAC\":\""
ifconfig wlan0 | grep 'HWaddr' | cut -d: -f2,3,4,5,6,7 | awk '{echo $3}'
echo "\",\"EthernetIP\":\""
ifconfig wlan0 | grep 'inet addr:' | cut -d: -f2 | awk '{ echo $1}'
echo "\",\"EthernetSubnet\":\""
ifconfig wlan0 | grep 'inet addr:' | cut -d: -f4
echo "\""