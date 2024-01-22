import * as React from 'react';
import { View, ScrollView, TextInput } from 'react-native';
import { SafeAreaView } from "react-native-safe-area-context";
import {Picker} from '@react-native-picker/picker';

import { showMessage } from "react-native-flash-message";
import NetInfo from "@react-native-community/netinfo";

// Import helper code
import { EditOrder, GetOrders, updateReservationStatus,DeleteOrderItem,  } from '../utils/Api';
import { PopupOk, PopupOkCancel } from "../utils/Popup";

// Import styling and components
import { TextParagraph, TextH1, TextH2, TextLabel } from "../components/StyledText";
import Styles from "../styles/MainStyle";
import { MyButton } from '../components/MyButton';

// Import styling and components
import { GetTables } from '../utils/Api';

export default function OrderScreen(props) {


  // State - data for this component

  // Data array, default to empty array
  const [orderItems, setOrderItems] = React.useState([])
  const [tableName, setTableName] = React.useState()
  const [tableId, setTableId] = React.useState()

  const [tables, setTables] = React.useState([])
  const [totalPrice, setTotalPrice] = React.useState()

  const [reservationNumber, setReservationNumber] = React.useState()
  // Set "effect" to retrieve and store data - only run on mount/unmount (loaded/unloaded)
  // "effectful" code is something that triggers a UI re-render
  //use effect is used to call method on page refresh(load)
  React.useEffect(refreshOrderItemsList, [])

  React.useEffect(refreshTableList, [])

  // Refresh the order list data - call the API
  function refreshOrderItemsList() {

    console.log("refresh order list")

    //const orderId = props.route.params.id
    const tableId = props.route?.params?.tableId??1
    //screen go to other screen//we should have a param to go to order screen
    setTableId(tableId)
    setTableName(props.route?.params?.tableName??"M1")
    //default M1
    setReservationNumber(props.route?.params?.reservationId)
    // Get data from the API
    
    getOrdersByTableId(tableId);

  }


  function getOrdersByTableId(tableId) {
    GetOrders(tableId)
    // Success
    .then(data => {
      // Store results in state variable
      setOrderItems(data.orderItems)
      setOrderTotalPrice(data.orderItems)
    })
    // Error
    .catch(error => {
      PopupOk("API Error", "Could not get order from the server")
    })
  }



  //Delete Order Item

  async function Delete(orderItem) {

    
    //Display flash message when there is a connection issue
   
    
    //cancel if no internet connection 
    if(!(await NetInfo.fetch()).isConnected)return

    PopupOkCancel(
      "Delete item?",
      `Are you sure you want to delete ${orderItem.menuItem.name}?`,
      () => {

     
        DeleteOrderItem(tableId,orderItem.id)
          .then(data => {

            PopupOk("Item deleted", ` ${orderItem.menuItem.name} has been deleted.`)
            getOrdersByTableId(tableId)
          })
          .catch(error => {

            PopupOk("Error", error)
          })

      },


    

      //cancel do nothing
      () => {

        //console.log("Cancel-no delete for you")
      }

    )
  }

  function refreshTableList() {

    console.log("refresh table list")

    // Get data from the API
    GetTables()
      // Success
      .then(data => {
        // Store results in state variable
        setTables(data)
        //setTableId(data[0].id)
      })
      // Error
      .catch(error => {
        PopupOk("API Error", "Could not get tables from the server")
      })

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


  async function UpdateQuanity(o,isIncrement) {
    var newOrderItem=orderItems.find(orderItem=>orderItem.id == o.id)
    
    let newOrderItems = [];

    if(isIncrement==true)
    {
      newOrderItem.qty=newOrderItem.qty+1
  
    }
    else
    {
      newOrderItem.qty=newOrderItem.qty-1
    }


    orderItems.forEach(orderItem => {
   
        if(orderItem.id == newOrderItem.id) {
          newOrderItems.push(newOrderItem)
        }
        else {
          newOrderItems.push(orderItem)
        }

    });

    setOrderItems(newOrderItems)
    setOrderTotalPrice(newOrderItems);
    
    await EditOrder(tableId, newOrderItems);

  }

  function displayTables() {
    
    return tables?.map(table => {
        return <Picker.Item key={table.id} label={table.name} value={table.id} />
      });    
  }

  function handleChangeTable(tableId) {
    setTableId(tableId);
    let table = tables.find(t => t.id == tableId)
    setTableName(table.name);
    getOrdersByTableId(tableId);
  }

  function setOrderTotalPrice(orderItems){
    var SumPrice = 0
    for (let i = 0; i < orderItems.length; i++) {
     SumPrice = Number(orderItems[i].menuItem.price)* Number(orderItems[i].qty) + SumPrice
    }
    setTotalPrice(SumPrice)

  }


  function UpdateNote(o, text){
    const newOrderItems = orderItems.map((item) =>
            item.menuItem.id === o.menuItem.id ? { ...item, notes: text } : item
          );
      setOrderItems(newOrderItems);
      EditOrder(tableId, newOrderItems);
  }

  function completeOrder() {
    updateReservationStatus(reservationNumber, 'Completed')
    //when press compeleted button changed the status compeleted
  }

  
  // Display all order items data
  function displayOrderItems() {
    //Display flash message when there's connection issue
    displayConnectionMessage()


    if (!orderItems) return

    // Loop through each item and turn into appropriate output and then return the result
    return orderItems.map(o => {

      // Create an output view for each item
      return (
        <View key={o.menuItem.id} style={Styles.personListItem}>
          <View style={Styles.personListItemDetails}>
            <TextParagraph style={Styles.personListItemName}>{o.menuItem.name}</TextParagraph>
            <TextParagraph style={Styles.personListItemText}>{`$ ${o.menuItem.price}`}</TextParagraph>
            <TextParagraph style={Styles.personListItemText}>{o.qty??"1"}</TextParagraph>
            <TextParagraph style={Styles.personListItemText}>{o.menuItem.description}</TextParagraph>

            {/* ... Existing code ... */}
            <TextInput
                  placeholder="Add a note"
                  value={o.notes}
                  onChangeText={(text) => {UpdateNote(o, text)}}
                  style={Styles.textInput}
            />
            

            
            <View key={o.menuItem.id} style={Styles.OrderQuantityRow}>
              <MyButton
                text="+"
                type="minor"      // default*|major|minor
                size="small"      // small|medium*|large
          
   
                buttonStyle={Styles.updateQuantity}
                textStyle={Styles.personListItemButtonText}
                onPress={() => UpdateQuanity(o,true)}
              />

              <TextInput value={o.qty} style={Styles.textInput}/>

              <MyButton
                text="-"
                type="minor"      // default*|major|minor
                size="small"      // small|medium*|large

                buttonStyle={Styles.updateQuantity}
                textStyle={Styles.personListItemButtonText}
                onPress={() => UpdateQuanity(o,false)}
              />
      
            </View>

            <MyButton
              text="Delete"
              type="minor"      // default*|major|minor
              size="small"      // small|medium*|large
              onPress={() => Delete(o)}
              buttonStyle={Styles.personListItemButton}
              textStyle={Styles.personListItemButtonText}
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

        <TextH1 style={{ marginTop: 0 }}>Your order list</TextH1>

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

        <TextH1 style={{ marginTop: 0 }}>Your table is: {tableName}</TextH1>
        
        <TextH1 style={{ marginTop: 0 }}>You are placing order for reservation number: {reservationNumber}</TextH1>
       
        <View style={Styles.personList}>
          {displayOrderItems()}
        </View>
        
        <TextH1 style={{ marginTop: 0 }}>Your Total Price is: {totalPrice}</TextH1>


          <MyButton
              text="Send to the Kitchen"
              type="major"      // default*|major|minor
              size="large"      // small|medium*|large
              onPress={() => completeOrder()}
              buttonStyle={Styles.personListItemButton}
              textStyle={Styles.personListItemButtonText}
            />
      </ScrollView>
    </SafeAreaView>
  );
}