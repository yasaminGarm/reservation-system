import { NavigationContainer } from '@react-navigation/native';
import { StatusBar } from 'expo-status-bar';
import { SafeAreaProvider } from "react-native-safe-area-context";
// import { StyleSheet, Text, View } from 'react-native';

// Import helpers and navigation
import RootNavigator from './navigation/RootNavigator';
import LinkingConfiguration from './navigation/LinkingConfiguration';
import FlashMessage from 'react-native-flash-message';


export default function App() {
  return (


    <SafeAreaProvider>

      <NavigationContainer linking={LinkingConfiguration}>
        <RootNavigator />
      </NavigationContainer>

      <StatusBar style="auto" />
      {/* Global FlashMessage component to allow banners to appear */}
      <FlashMessage position="top"/>



    </SafeAreaProvider>


  );
}





/*import { NavigationContainer } from '@react-navigation/native';
import { StatusBar } from 'expo-status-bar';
import { SafeAreaProvider } from "react-native-safe-area-context";
// import { StyleSheet, Text, View } from 'react-native';
import React, { useState } from 'react';

// Import helpers and navigation
import RootNavigator from './navigation/RootNavigator';
import LinkingConfiguration from './navigation/LinkingConfiguration';
import FlashMessage from 'react-native-flash-message';
import HomeScreen from './screens/HomeScreen';
import { AuthContext } from './utils/AuthContext';


export default function App() {
  const [token, setToken] = useState(null)

  React.useEffect(() => {
    // Fetch the token from storage then navigate to our appropriate place
    const bootstrapAsync = async () => {
      let userToken;

      try {
        userToken = await getToken();
      } catch (e) {
        // Restoring token failed
      }

      // After restoring token, we may need to validate it in production apps

      // This will switch to the App screen or Auth screen and this loading
      // screen will be unmounted and thrown away.
      setToken(userToken);
    };

    bootstrapAsync();
  }, []);
  const [isAuthenticated, setIsAuthenticated] = useState(false);


  return (
    <AuthContext.Provider value={setToken}>
    <SafeAreaProvider>
      <NavigationContainer linking={LinkingConfiguration}>
        {isAuthenticated ? ( 
          <RootNavigator />
        ):(
          <HomeScreen onLogin={() => {
            setIsAuthenticated(true);
            const navigation = useNavigation();
            navigation.navigate('OrderScreen'); // Navigate to the OrderScreen
          }} />
        )}
        


      </NavigationContainer>
      <StatusBar style="auto" />
      {/* Global FlashMessage component to allow banners to appear }
      <FlashMessage position="top"/>
    </SafeAreaProvider>
    </AuthContext.Provider>
  );
} */

