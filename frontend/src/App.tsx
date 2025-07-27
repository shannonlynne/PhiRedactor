import { useEffect } from "react";

function App() {
  useEffect(() => {
    document.title = "PHI Redactor";
  }, []);

  return <div className="App" style={{ padding: "2rem" }}></div>;
}

export default App;
