// import "./App.css";
import RootProvider from "./providers/RootProvider";
import Router from "./router/Router";

function App() {
  return (
    <RootProvider>
      <Router />
    </RootProvider>
  );
}

export default App;
