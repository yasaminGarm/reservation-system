import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import * as React from 'react';

// Import styling and components
import TabBarIcon from '../components/TabBarIcon';
import Colours from "../constants/Colours";
import Styles from "../styles/MainStyle";

// Import navigators & screens
import HelpScreen from '../screens/HelpScreen';
import OrderingNavigator from './OrderingNavigator';
import OrderScreen from '../screens/OrderScreen';
import LogoutScreen from '../screens/LogoutScreen';

const BottomTab = createBottomTabNavigator();
const INITIAL_ROUTE_NAME = 'Home';

export default function BottomTabNavigator({ navigation, route }) {

  return (
    <BottomTab.Navigator
      initialRouteName={INITIAL_ROUTE_NAME}
      screenOptions={{
        headerShown: false,
        tabBarActiveTintColor: Colours.tabLabelSelected,
        tabBarInactiveTintColor: Colours.tabLabel,
        tabBarStyle: Styles.navBar,
        tabBarLabelStyle: Styles.navBarLabel,
      }}
    >
   
      <BottomTab.Screen
        name="Menu"
        component={OrderingNavigator}
        options={{
          title: 'View Menu',
          unmountOnBlur: true,   // Reset the screen when it loses focus (when someone navigates away from it)
          tabBarIcon: ({ focused }) => <TabBarIcon focused={focused} name="md-restaurant" />
        }}
      />
      <BottomTab.Screen
        name="Order"
        component={OrderScreen}
        options={{
          title: 'view Order',
          unmountOnBlur: true,   // Reset the screen when it loses focus (when someone navigates away from it)
          tabBarIcon: ({ focused }) => <TabBarIcon focused={focused} name="md-basket" />
        }}
      />
      <BottomTab.Screen
        name="Help"
        component={HelpScreen}
        options={{
          title: 'Help',
          tabBarIcon: ({ focused }) => <TabBarIcon focused={focused} name="md-help-circle" />,
        }}
      />

      <BottomTab.Screen
        name="Logout"
        component={LogoutScreen}
        options={{
          title: 'Log out',
          unmountOnBlur: true,   // Reset the screen when it loses focus (when someone navigates away from it)
          tabBarIcon: ({ focused }) => <TabBarIcon focused={focused} name="md-log-out" />
        }}
        />


    </BottomTab.Navigator>


  );
}