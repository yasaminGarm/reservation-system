import * as Linking from 'expo-linking';

export default {
  prefixes: [Linking.createURL('/')],
  config: {
    screens: {
      Root: {
        path: '/',
        screens: {
          //Home: 'home',
          menu: {
            path: 'StaffDashboard/Menu',
            screens: {
              ViewMenu: 'ViewMenu',
            },
          },
          AddItemToOrder: 'AddItemToOrder',
          Order: 'StaffDashboard/Order',
          Help: 'StaffDashboard/help',
        },
      },
      NotFound: '*', // catch-all route (404 resource not found)

    },
  },
};

