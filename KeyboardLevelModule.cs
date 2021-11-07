using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace GPSTool
{
    public class KeyboardLevelModule : LevelModule
    {
        private GameObject keyboard;
        AsyncOperationHandle<GameObject> handleKeyboard;
        public Transform transKeyboard;
        private bool previousStateKeyboard = false;
        private bool currentStateKeyboard = false;
        private bool errorInput = false;
        private Button button0;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button buttonDot;
        private Button buttonEnter;
        private Button buttonPlusMinus;
        private Button buttonDelete;
        private Button buttonClear;
        private float valueToAssignedToPosition = 0.0f;
        GameObject keyboardCanvas;
        private Text txtInput;
        private GPSToolController gPSToolController;

        public override IEnumerator OnLoadCoroutine()
        {
            gPSToolController = GameManager.local.gameObject.GetComponent<GPSToolController>();
            return base.OnLoadCoroutine();
        }
        public override void Update()
        {
            if (gPSToolController == null)
            {
                gPSToolController = GameManager.local.gameObject.GetComponent<GPSToolController>();
                return;
            }
            else
            {
                if (keyboard == null && Player.currentCreature != null)
                {
                    // Creates the Keyboard
                    handleKeyboard = Addressables.LoadAssetAsync<GameObject>("Neeshka.GPSTool.Keyboard");
                    keyboard = handleKeyboard.WaitForCompletion();
                    keyboard = UnityEngine.Object.Instantiate(keyboard);
                    keyboard.SetActive(false);
                    keyboard.transform.position = (Player.local.creature.transform.position + new Vector3(0.0f, 2.0f, 0.0f));
                    keyboardCanvas = keyboard.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
                    txtInput = keyboard.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
                    button7 = keyboardCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
                    button8 = keyboardCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
                    button9 = keyboardCanvas.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();
                    button4 = keyboardCanvas.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
                    button5 = keyboardCanvas.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
                    button6 = keyboardCanvas.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();
                    button1 = keyboardCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
                    button2 = keyboardCanvas.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
                    button3 = keyboardCanvas.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();
                    buttonPlusMinus = keyboardCanvas.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
                    button0 = keyboardCanvas.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
                    buttonDot = keyboardCanvas.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();
                    buttonEnter = keyboardCanvas.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
                    buttonClear = keyboardCanvas.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
                    buttonDelete = keyboardCanvas.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.GetComponent<Button>();
                    // Add an event listener for buttons
                    button7.onClick.AddListener(ClickButton7);
                    button8.onClick.AddListener(ClickButton8);
                    button9.onClick.AddListener(ClickButton9);
                    button4.onClick.AddListener(ClickButton4);
                    button5.onClick.AddListener(ClickButton5);
                    button6.onClick.AddListener(ClickButton6);
                    button1.onClick.AddListener(ClickButton1);
                    button2.onClick.AddListener(ClickButton2);
                    button3.onClick.AddListener(ClickButton3);
                    button0.onClick.AddListener(ClickButton0);
                    buttonPlusMinus.onClick.AddListener(ClickButtonPlusMinus);
                    buttonDot.onClick.AddListener(ClickButtonDot);
                    buttonEnter.onClick.AddListener(ClickButtonEnter);
                    buttonClear.onClick.AddListener(ClickButtonClear);
                    buttonDelete.onClick.AddListener(ClickButtonDelete);
                    // Initialization of datas
                    gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet = false;
                    gPSToolController.data.PlayerTeleportPositionXButtonPressedGetSet = false;
                    gPSToolController.data.PlayerTeleportPositionYButtonPressedGetSet = false;
                    gPSToolController.data.PlayerTeleportPositionZButtonPressedGetSet = false;
                    gPSToolController.data.PlayerTeleportPositionDistanceMaxButtonPressedGetSet = false;
                }

                if (Player.currentCreature != null)
                {
                    currentStateKeyboard = (gPSToolController.data.PlayerTeleportPositionXButtonPressedGetSet
                                            ^ gPSToolController.data.PlayerTeleportPositionYButtonPressedGetSet
                                            ^ gPSToolController.data.PlayerTeleportPositionZButtonPressedGetSet
                                            ^ gPSToolController.data.PlayerTeleportPositionDistanceMaxButtonPressedGetSet) && !keyboard.activeSelf;
                    // Rising edge
                    if (keyboard != null && currentStateKeyboard != previousStateKeyboard && previousStateKeyboard == false)
                    {
                        keyboard.SetActive(true);
                        transKeyboard = keyboard.transform;
                        keyboard.layer = 5;
                    }
                    // Deactivate the keyboard
                    if (gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet == true && keyboard.activeSelf)
                    {
                        keyboard.SetActive(false);
                    }
                    // Set the new state
                    previousStateKeyboard = currentStateKeyboard;
                    if (keyboard.activeSelf)
                    {
                        transKeyboard.position = MenuBook.local.gameObject.transform.position + new Vector3(0.0f, 0.4f, 0.0f);
                        transKeyboard.rotation = MenuBook.local.gameObject.transform.rotation;
                    }
                }
            }
        }
        public void ClickButton7()
        {
            if (errorInput == false)
            {
                txtInput.text += "7";
            }
            else
            {
                txtInput.text = "7";
                errorInput = false;
            }
        }
        public void ClickButton8()
        {
            if (errorInput == false)
            {
                txtInput.text += "8";
            }
            else
            {
                txtInput.text = "8";
                errorInput = false;
            }
        }
        public void ClickButton9()
        {
            if (errorInput == false)
            {
                txtInput.text += "9";
            }
            else
            {
                txtInput.text = "9";
                errorInput = false;
            }
        }
        public void ClickButton4()
        {
            if (errorInput == false)
            {
                txtInput.text += "4";
            }
            else
            {
                txtInput.text = "4";
                errorInput = false;
            }
        }
        public void ClickButton5()
        {
            if (errorInput == false)
            {
                txtInput.text += "5";
            }
            else
            {
                txtInput.text = "5";
                errorInput = false;
            }
        }
        public void ClickButton6()
        {
            if (errorInput == false)
            {
                txtInput.text += "6";
            }
            else
            {
                txtInput.text = "6";
                errorInput = false;
            }
        }
        public void ClickButton1()
        {
            if (errorInput == false)
            {
                txtInput.text += "1";
            }
            else
            {
                txtInput.text = "1";
                errorInput = false;
            }
        }
        public void ClickButton2()
        {
            if (errorInput == false)
            {
                txtInput.text += "2";
            }
            else
            {
                txtInput.text = "2";
                errorInput = false;
            }
        }
        public void ClickButton3()
        {
            if (errorInput == false)
            {
                txtInput.text += "3";
            }
            else
            {
                txtInput.text = "3";
                errorInput = false;
            }
        }
        public void ClickButtonPlusMinus()
        {
            if (txtInput.text.StartsWith("-"))
            {
                // Remove the - at the beginning of the string
                txtInput.text = txtInput.text.Remove(0, 1);
            }
            else
            {
                // Add the - at the beginning of the string
                txtInput.text = txtInput.text.Insert(0, "-");
            }
        }
        public void ClickButton0()
        {
            if (errorInput == false)
            {
                txtInput.text += "0";
            }
            else
            {
                txtInput.text = "0";
                errorInput = false;
            }
        }
        public void ClickButtonDot()
        {
            if (errorInput == false)
            {
                txtInput.text += ".";
            }
            else
            {
                txtInput.text = ".";
                errorInput = false;
            }
        }
        public void ClickButtonEnter()
        {
            // Try if it's a float else display a message
            if (float.TryParse(txtInput.text, out valueToAssignedToPosition))
            {
                gPSToolController.data.PlayerTeleportPositionValueToAssignedGetSet = valueToAssignedToPosition;
                gPSToolController.data.KeyboardFinishEnterButtonPressedGetSet = true;
                txtInput.text = "";
            }
            else
            {
                txtInput.text = "Error, wrong format entered";
            }
        }
        public void ClickButtonClear()
        {
            // Clear the string
            txtInput.text = "";
        }
        public void ClickButtonDelete()
        {
            // Remove the last element of the string
            if (txtInput.text.Length != 0)
            {
                txtInput.text = txtInput.text.Remove(txtInput.text.Length - 1, 1);
            }
        }
    }
}
