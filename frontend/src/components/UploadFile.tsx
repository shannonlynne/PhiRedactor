import { styled } from "@mui/material/styles";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import { Box, Typography, Button, Snackbar, Alert } from "@mui/material";
import redactionPost from "../services/redactionPost";
import React from "react";

const VisuallyHiddenInput = styled("input")({
  clip: "rect(0 0 0 0)",
  clipPath: "inset(50%)",
  height: 1,
  overflow: "hidden",
  position: "absolute",
  bottom: 0,
  left: 0,
  whiteSpace: "nowrap",
  width: 1,
});

const UploadFile: React.FC = () => {
  const handleFileUpload = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const files: File[] = Array.from(event.target.files || []);

    var response = await redactionPost(files);
    setToastSeverity(response.success ? "success" : "error");
    setToastMessage(response.message);
    setToastOpen(true);
    console.log(response.message);

    event.target.value = "";
  };

  const [toastOpen, setToastOpen] = React.useState(false);
  const [toastMessage, setToastMessage] = React.useState("");
  const [toastSeverity, setToastSeverity] = React.useState<"success" | "error">(
    "success"
  );

  return (
    <div>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: 2,
          paddingTop: "5rem",
        }}
      >
        <Typography>Upload one or more Lab Orders in .txt format</Typography>

        <Button
          component="label"
          role={undefined}
          variant="contained"
          tabIndex={-1}
          startIcon={<CloudUploadIcon />}
        >
          Upload
          <VisuallyHiddenInput
            type="file"
            accept=".txt"
            onChange={handleFileUpload}
            multiple
          />
        </Button>
      </Box>
      <Snackbar
        open={toastOpen}
        autoHideDuration={6000}
        onClose={() => setToastOpen(false)}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={() => setToastOpen(false)}
          severity={toastSeverity}
          variant="filled"
          sx={{ width: "100%", justifyContent: "center", textAlign: "center" }}
        >
          <Typography sx={{ fontWeight: 500 }}>{toastMessage}</Typography>
        </Alert>
      </Snackbar>
    </div>
  );
};

export default UploadFile;
