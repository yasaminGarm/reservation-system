import { createStackNavigator } from '@react-navigation/stack';
import * as React from 'react';

// Import navigation and screens
import ViewMenuScreen from '../screens/ViewMenuScreen';
import ViewMenuItemScreen from '../screens/ViewMenuItemScreen';

// Import styling and components
import Styles from "../styles/MainStyle";

const Stack = createStackNavigator();

export default function OrderingNavigator() {
  return (
    <Stack.Navigator 
      initialRouteName="ViewMenu"
      screenOptions={{
        headerShown: true,
        headerBackTitle: "Back",
        headerStyle: Styles.headerBar,
        headerTitleStyle: Styles.headerBarTitle,
      }}>
      <Stack.Screen
        name="ViewMenu"
        component={ViewMenuScreen}
        options={{ title: 'View Menu' }} />
      <Stack.Screen
        name="ViewMenuItem"
        component={ViewMenuItemScreen}
        options={{ title: 'View Menu Item' }} />

    </Stack.Navigator>
  );
}