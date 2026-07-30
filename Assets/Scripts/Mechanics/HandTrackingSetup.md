# Meta Quest Hand Tracking Setup Guide

## Required Packages
- XR Plugin Management
- OpenXR
- XR Hands
- XR Interaction Toolkit
- Meta XR Core SDK (recommended)

## Project Settings
1. Edit → Project Settings → XR Plug-in Management
2. Enable **OpenXR**
3. Under OpenXR → Enable **Meta Quest Support**
4. Add the **Hand Tracking** feature if available

## Scene Setup
1. Create an empty GameObject named `HandGestureManager`
2. Attach the `QuestHandGestureManager` script
3. Make sure your XR Origin / Camera Rig is present in the scene

## Gesture Mapping
| Gesture                    | Action                      |
|---------------------------|-----------------------------|
| Right Hand Pinch          | Voidblade Attack            |
| Left Hand Swipe Right     | Switch to Ravager Stance    |
| Left Hand Swipe Left      | Switch to Sentinel Stance   |
| Both Hands Open Palm      | Dimensional Phase Shift     |

## Tips
- Test on actual Quest hardware (hand tracking is limited in Link/simulator)
- Adjust `pinchThreshold` and `swipeDistanceThreshold` in the Inspector if needed
- Add haptic feedback later using OVRInput or OpenXR haptics
