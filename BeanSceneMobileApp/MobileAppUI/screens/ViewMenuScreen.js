import * as React from 'react';
import { View, ScrollView, TouchableOpacity,Image, StyleSheet, Text } from 'react-native';
import { SafeAreaView } from "react-native-safe-area-context";
import { showMessage } from "react-native-flash-message";
import NetInfo from "@react-native-community/netinfo";

// Import helper code
import { GetProducts, AddToOrderApi,GetTables, updateReservationStatus, GetMatchingReservationByTableId } from '../utils/Api';
import { PopupOk, PopupOkCancel } from "../utils/Popup";

// Import styling and components
import { TextParagraph, TextH1, TextH2, TextLabel } from "../components/StyledText";
import Styles from "../styles/MainStyle";
import { MyButton } from '../components/MyButton';
import {Picker} from '@react-native-picker/picker';



export default function ViewMenuScreen(props) {


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

  // Data array, default to empty array
  const [products, setProducts] = React.useState([])
  const [tables, setTables] = React.useState([])

  const [tableId, setTableId] = React.useState(0)

  const [reservation, setReservation] = React.useState()

  // Set "effect" to retrieve and store data - only run on mount/unmount (loaded/unloaded)
  // "effectful" code is something that triggers a UI re-render
  React.useEffect(refreshTableList, [])


  React.useEffect(refreshMenuList, [])
  
  // Refresh the menu list data - call the API
  function refreshMenuList() {

    console.log("refresh menu list")

    // Get data from the API
    GetProducts()
      // Success
      .then(data => {
        // Store results in state variable
        setProducts(data)
      })
      // Error
      .catch(error => {
        PopupOk("API Error", "Could not get MenuItem from the server")
      })

  }

  function refreshTableList() {

    console.log("refresh table list")

    // Get data from the API
    GetTables()
      // Success
      .then(data => {
        // Store results in state variable
        setTables(data)
        setTableId(data[0].id)
      })
      // Error
      .catch(error => {
        PopupOk("API Error", "Could not get tables from the server")
      })

  }


  function showViewMenuItem(menuItem) {

    props.navigation.navigate("ViewMenuItem", { id: menuItem.id })

  }

  function AddToOrder(menuItem) {
    AddToOrderApi(tableId, menuItem)
    .then(data => {
        
          //props.navigation.replace('Root', { screen: 'Order' });
        let table=tables.find(t=>t.id == tableId)
          props.navigation.navigate("Order", { tableId: tableId,tableName:table.name, reservationId: reservation.id })
            
        
    }).catch(error => {
      
    })

  }


  function displayTables() {
    
    return tables?.map(table => {
        return <Picker.Item key={table.id} label={table.name} value={table.id} />
      });    
  }


  //Display flash message banner if offline
  function displayConnectionMessage() {
    //Get Network connection status
    NetInfo.fetch().then(status => {
      //check if not connected to the internet
      if (!status.isConnected) {
        // if(true){
        //Display the flash message
        showMessage({

          message: "No  internet connection",
          description: "You will only see cached data until you \nhave an active internet connection again.",
          type: "warning",
          duration: 5000,
          floating: true,
          icon: "warning",
          autoHide: true,
        })
      }
    })
  }


  function handleChangeTable(tableId) {
    setTableId(tableId);
    let table = tables.find(t => t.id == tableId)
    //setTableName(table.name);
    

    //displaying user name taking order for x?
    GetMatchingReservationByTableId(tableId)
    .then(data => {
        
      PopupOkCancel(
        "Placing Order?",
        `Are you placing Order for reservation ${data.firstName} ${data.lastName}?`,
        () => {
  
            setReservation(data);

            updateReservationStatus(data.id, 'Seated')
            //asking are you taking order for x?if yes change the status for x to seated
                 
  
            })
  
  
            .catch(error => {
  
              PopupOk("Error", error)
            })
        
    }).catch(error => {
      
    })

  }
 
  
  // Display all products data
  function displayProducts() {
    //Display flash message when there's connection issue
    displayConnectionMessage()


    if (!products) return
   
    // Loop through each item and turn into appropriate output and then return the result
    return products.map(p => {

      // Create an output view for each item
      return (
        <View key={p.id} style={Styles.personListItem}>
          <View style={Styles.personListItemDetails}>
            
            <TextParagraph style={Styles.personListItemName}>{p.name}</TextParagraph>
            <TextParagraph style={Styles.personListItemText}>{p.category?.name ?? "---"}</TextParagraph>
            <TextParagraph style={Styles.personListItemText}> {`$ ${p.price}`}</TextParagraph>

            <Image
                style={styles.stretch}
                source={require("../assets/images/products/"+p.imageName)}
                onError={({ currentTarget }) => {
                currentTarget.onerror = null; // prevents looping
                currentTarget.src="../assets/images/products/general.jpg";
                }}
            />
         
            

          </View>
          <View style={Styles.personListItemButtons}>

          <MyButton
              text="View Menu Item"
              type="major"      // default*|major|minor
              size="small"      // small|medium*|large
              buttonStyle={Styles.personListItemButton}
              textStyle={Styles.personListItemButtonText}
              onPress={() => showViewMenuItem(p)}
            />



            <MyButton
              text="Add To Order"
              type="default"      // default*|major|minor
              size="small"      // small|medium*|large
              buttonStyle={Styles.personListItemButton}
              textStyle={Styles.personListItemButtonText}
              onPress={() => AddToOrder(p)}
            />

          </View>
        </View>
      )

    })

  }


  // Main output of the screen component
  return (
    <SafeAreaView style={Styles.safeAreaView}>

      <ScrollView style={Styles.container} contentContainerStyle={Styles.contentContainer}>

        <TextH1 style={{ marginTop: 0 }}>Listing all menu items</TextH1>

        <View style={Styles.formRow}>
                <TextLabel>Tables:</TextLabel>
                {/*<TextInput value={departmentId} onChangeText={setDepartmentId} style={Styles.textInput}/>*/}
                <Picker 
                  selectedValue={tableId}
                  onValueChange={handleChangeTable}
                  style={Styles.picker}
                  itemStyle={Styles.pickerItem}
                >
                  {displayTables()}
                </Picker>
        </View>

        <View style={Styles.personList}>
          {displayProducts()}
        </View>

      </ScrollView>
    </SafeAreaView>
  );
}

