import UploadFile from "./components/UploadFile";
import { useEffect } from "react";
import { Typography } from "@mui/material";

function App() {
  useEffect(() => {
    document.title = "PHI Redactor";
  }, []);

  return (
    <div className="App" style={{ padding: "2rem" }}>
      <Typography
        variant="h4"
        component="h1"
        align="center"
        sx={{ color: "#78909C" }}
      >
        PhiRedactor
      </Typography>
      <UploadFile />
    </div>
  );
}

export default App;
