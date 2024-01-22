import * as React from 'react';
import { View, ScrollView, Pressable } from 'react-native';
import { SafeAreaView } from "react-native-safe-area-context";

// Import helper code
import Settings from '../constants/Settings';


// Import styling and components
import { TextParagraph, TextH1, TextH2, TextH3, TextListItem } from "../components/StyledText";
import Styles from "../styles/MainStyle";
import { MyButton } from '../components/MyButton';
import Colours from '../constants/Colours';

export default function HelpScreen(props) {


  const[fontSizeModifier,setFontSizeModifier]=React.useState(Settings.fontSizeModifier)


    function changeFontSize(sizeModifier){
      Settings.fontSizeModifier+=sizeModifier
      setFontSizeModifier(Settings.fontSizeModifier)

    }

  return (
    <SafeAreaView style={Styles.safeAreaView}>
      <ScrollView style={Styles.container} contentContainerStyle={Styles.contentContainer}>

        <View>
          
          <TextH1 style={{marginTop:0}}>Help topics</TextH1>
          <TextH2>Change settings</TextH2>
          <TextParagraph>Here are some basic settings to change to make the app more comfortable to use.</TextParagraph>
          <TextH3>Font size</TextH3>


          <View style={Styles.helpButtonContainer}>
          <MyButton
            text="- Smaller"
            type="default"      // default*|major|minor
            size="medium"      // small|medium*|large
            onPress={()=>changeFontSize(-0.1)}
            buttonStyle={Styles.helpButton}
          />


          <MyButton
            text="+ Bigger"
            type="defualt"      // default*|major|minor
            size="medium"      // small|medium*|large
            onPress={()=>changeFontSize(+0.1)}
            buttonStyle={Styles.helpButton}
          />

        </View>




          <TextH2>Sample content</TextH2>

          <TextParagraph>Here is some sample content for a help topic (or just any set of static text for the screen).</TextParagraph>

          <TextH2>Custom components</TextH2>

          <TextParagraph>You can add all of your own content and structure for these screens, making use of the customised text components from components/StyledText.</TextParagraph>

          <TextH3>Custom text components:</TextH3>

          <TextListItem>MonoText - Monospaced font (Space Mono)</TextListItem>
          <TextListItem>TextH1 - heading 1</TextListItem>
          <TextListItem>TextH2 - heading 2</TextListItem>
          <TextListItem>TextH3 - heading 3</TextListItem>
          <TextListItem>TextParagraph - paragraph</TextListItem>
          <TextListItem>TextListItem - bullet list item</TextListItem>
          <TextListItem>TextLabel - form label (inline with input)</TextListItem>

          <TextH2>Wanna go home?</TextH2>

          <Pressable onPress={() => props.navigation.replace('Root')}>
            <TextParagraph style={{marginVertical: 10, color: Colours.tabLabelSelected}}>Click here to go home...</TextParagraph>
          </Pressable>

        </View>
      </ScrollView>
    </SafeAreaView>
  );
}