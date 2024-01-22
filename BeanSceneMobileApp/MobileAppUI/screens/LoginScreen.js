import * as React from 'react';
import { Image, Text, View, ScrollView, Pressable, TextInput } from 'react-native';
import { SafeAreaView } from "react-native-safe-area-context";
// Import styling and components
import Styles from "../styles/MainStyle";
import { MyButton } from '../components/MyButton';
import { TextLabel, TextH3  } from "../components/StyledText";
import { Login } from '../utils/Api';

export default function LoginScreen(props) {
  //State management
  const [isLogoColour, setIsLogoColour] = React.useState(true);

  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  
  const [error, setError] = React.useState(false);



  function toggleLogo() {
    setIsLogoColour(!isLogoColour)
  }

  function login() {
    
    Login({username,password})
    .then(data => {
        if(data==true)
        {
          //props.navigation.replace('Root', { screen: 'Menu' });
          props.navigation.navigate('StaffDashboard', { screen: 'Menu' });
        }
        else
        {
            setError(true)
        }
    }).catch(error => {
      
    })
    
  
  }


  return (
    <SafeAreaView style={Styles.safeAreaView}>
      
        {/* Logo */}
        <View style={Styles.homeLogoContainer}>
          <Pressable onPress={toggleLogo}>
            <Image
              //condotion
              source={
                isLogoColour
                  ? require("../assets/images/logoRest.jpg")
                  : require("../assets/images/logoRest.jpg")

              }
              style={Styles.homeLogo}
            />
          </Pressable>
        </View>


        {/* Heading */}
        <View style={Styles.homeHeadingContainer}>
          <Text style={Styles.homeHeading}> Login <br></br> BeanScene  </Text>
        </View>

        <View style={Styles.fieldSet}>
          <View style={Styles.formRow}>
            <TextLabel>Username:</TextLabel>
            <TextInput value={username} onChangeText={setUsername} style={Styles.textInput} />
          </View>

          <View style={Styles.formRow}>
            <TextLabel>Password:</TextLabel>
            <TextInput value={password} onChangeText={setPassword} style={Styles.textInput}  secureTextEntry={true}/>
          </View>

          <View style={Styles.homeButtonContainer}>
            <MyButton
              text="Login"
              type="major"      // default*|major|minor
              size="large"      // small|medium*|large
              onPress={login}
              buttonStyle={Styles.loginButton}
            />

          </View>

          {error && <TextH3 style={{color:'red'}}>Username or password is wrong</TextH3>}

        </View>
      
    </SafeAreaView>
  );
}