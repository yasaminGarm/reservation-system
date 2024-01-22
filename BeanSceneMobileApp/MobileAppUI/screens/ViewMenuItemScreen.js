import * as React from 'react';
import { View, ScrollView } from 'react-native';
import { SafeAreaView } from "react-native-safe-area-context";

// Import helper code
import Settings from '../constants/Settings';
import { GetProductById, GetProducts } from '../utils/Api';
import { PopupOk, PopupOkCancel } from "../utils/Popup";

// Import styling and components
import { TextParagraph, TextH1, TextH2, TextLabel } from "../components/StyledText";
import Styles from "../styles/MainStyle";
import {Image, StyleSheet} from 'react-native';




export default function ViewMenuItemScreen(props) {



  const MenuItemTemplate = { 
    id: 0,
    name: "",
    description : "",
    price:0 ,
    category: {name: "", id: 0},
    ingredient: "",
    dietary: "",

    imageName:""

  }
  const styles = StyleSheet.create({
    container: {
      paddingTop: 50,
    },
    stretch: {
      width: 100,
      height: 100,
      
    },
  });




  // State - data for this component

  const [MenuItem, setMenuItem] = React.useState(MenuItemTemplate)

  // Set "effect" to retrieve and store data - only run on mount/unmount (loaded/unloaded)
  // "effectful" code is something that triggers a UI re-render
  React.useEffect(refreshMenuItem, [])

  // Refresh the person data - call the API
  function refreshMenuItem() {

    //Get menu item id id from route
    const MenuItemId = props.route.params.id

    // Get data from the API
    GetProductById(MenuItemId)
      // Success
      .then(data => {
        // Store results in state variable
        if(data) setMenuItem(data)
      })
      // Error
      .catch(error => {

        // Display error
        PopupOk("API Error", "Could not get menu item  from the server")

        //This is optional
        props.navigation.navigate("ViewMenu")

      })

  }


  if (!MenuItem) return
  // Main output of the screen component
  return (
    <SafeAreaView style={Styles.safeAreaView}>
      
      <ScrollView style={Styles.container} contentContainerStyle={Styles.contentContainer}>
          
        <TextH1 style={{marginTop:0}}>menuItem: {MenuItem.name}</TextH1>

        <View style={Styles.form}>
         
          <View style={Styles.fieldSet}>
              <TextParagraph style={Styles.legend}>Details</TextParagraph>
              <View style={Styles.formRow}>
                <TextLabel>Name:</TextLabel>
                <TextParagraph>{MenuItem.name}</TextParagraph>
              </View>
              <View style={Styles.formRow}>
                <TextLabel>Description:</TextLabel>
                <TextParagraph>{MenuItem.description}</TextParagraph>
              </View>
              <View style={Styles.formRow}>
                <TextLabel>Price:</TextLabel>
                <TextParagraph>{MenuItem.price}</TextParagraph>
              </View>
              <View style={Styles.formRow}>
                <TextLabel>Category:</TextLabel>
                <TextParagraph>{MenuItem.category?.name}</TextParagraph>
              </View>
              <View style={Styles.formRow}>
                <TextLabel>Ingredients:</TextLabel>
                <TextParagraph>{MenuItem.ingredient}</TextParagraph>
              </View>
              <View style={Styles.formRow}>
                <TextLabel>Dietary:</TextLabel>
                <TextParagraph>{MenuItem.dietary}</TextParagraph>
              </View>

              {/**load image only when MenuItem and MenuItem.imageName is not NULL */} 
             {MenuItem && MenuItem.imageName && 
            <Image
                style={styles.stretch}
                source={require("../assets/images/products/"+ MenuItem.imageName)}
                onError={({ currentTarget }) => {
    currentTarget.onerror = null; // prevents looping
    currentTarget.src="../assets/images/products/general.jpg";
  }}
            /> }
        
          </View> 
        </View> 
      </ScrollView>
    </SafeAreaView>
  );
}
