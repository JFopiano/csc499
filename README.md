# Data Science VR Game

## Overview
This virtual reality game, developed in Unity, provides an immersive educational experience focused on teaching Data Science. Players progress through a school environment with various rooms (levels), solving lessons crafted with a linear progression on how to learn key takeaways about data science.

---

## Game Structure
- Players begin in a school environment  
- Progression through 4 levels (with level 5 in development)  
- Each level contains Data Science related concepts to complete  
- Post-level questionnaires assess understanding  
- Doors open to new levels upon successful completion  

---

## Level Breakdown
1. **Level 1**: Introduction to basic controls/import your dataset (CSV file)  
2. **Level 2**: Data preprocessing (how to handle NaNs)  
3. **Level 3**: Univariate models  
4. **Level 4**: Bivariate models  
5. **Level 5**: Heatmaps *(in development)*  
6. **Level 6**: Scatter Plots  
7. **Level 7**: *(potential)* Analysis with and without outliers  

---

## Breast Cancer Dataset
The game uses the **Breast Cancer Wisconsin (Diagnostic)** dataset. Players analyze the following columns:

- `radius` (mean of distances from center to points on the perimeter)  
- `texture` (standard deviation of gray-scale values)  
- `perimeter`  
- `area`  
- `smoothness` (local variation in radius lengths)  
- `compactness` (perimeter² / area - 1.0)  
- `concavity` (severity of concave portions of the contour)  
- `concave points` (number of concave portions of the contour)  
- `symmetry`  
- `fractal dimension` ("coastline approximation" - 1)  

---

## Technical Implementation

### Core Architecture
The game follows a **manager-based architecture** with a central `GameManager` coordinating various specialized mini-managers.

---

## Key Components

### `GameManager` (`src/manager/GameManager.cs`)
- Central hub managing game state and player progression  
- Coordinates all mini-managers  
- Initializes level-specific SVMs  
- Tracks player progression through levels  
- Handles VR controller interactions  

---

### Mini-Managers
- **ArrowManager**: Guides player movement with visual cues  
- **DoorManager**: Controls door states for level progression  
- **TaskPopupManager**: Manages in-game task instructions  
- **UserDataManager**: Tracks player progress and survey responses  
- **SurveyManager**: Handles assessment questionnaires  

---

### Interactive Components (`src/scripts/`)
- **SolverButtons**: Implements level-specific solution buttons  
- **CreateSVMButtonScript**: Handles dynamic SVM creation in Level 4  
- **DecisionPlaneControlsScript**: Controls for manipulating decision boundaries  
- **StartButtonScript**: Handles game initialization  

---

## Data Management
Player progress and survey responses are persisted across sessions using **JSON serialization**.

---

## VR Implementation
- Built using **Unity's XR Interaction Toolkit**  
- Supports **left-hand task hologram** for convenient instruction viewing  
- Implements intuitive VR interactions with game elements  

---

## Future Development
- Level 5 implementation *(in progress)*  
- Level 6 implementation  
- Potentially level 7 implementation  
- Enhanced visualization techniques  
- End-of-class surveys that reflect the lesson plan for each classroom  

---

## Detailed Component Documentation

### GameManager
The `GameManager` serves as the **central orchestrator**, managing game state and coordinating all subsystems.

#### Responsibilities
- Initializes and manages all mini-managers (`initManagers()`)  
- Tracks player progression with boolean flags  
- Handles VR controller input (`CheckSecondaryButtonPressedOnLeftController()`)  
- Manages task hologram display  
- Controls player movement via `GameControlManager`  
- Coordinates level transitions and arrow systems  

#### Key Methods
- `startGame()`: Begins gameplay and level 1 arrows  
- `keepTryingToPutTaskHologramInPlayerLeftHand()`: Keeps task display with player  
- `Start()`: Initializes SVMs and game state  
- `Update()`: Monitors input to toggle task popup  

---

### Mini-Managers

#### ArrowManager
- Guides players with sequential arrow lighting  
- Different sequences per level  
- Feedback for direction and goals  

**Key Methods**:  
- `startLevel1ArrowLightingSequence()` to `startLevel4ArrowLightingSequence()`  
- `stopLevelNArrowLightingSequence()`  

#### DoorManager
- Locks/unlocks doors based on level completion and surveys  
- Visual feedback for door state  
- Physical progression gates  

#### TaskPopupManager
- Displays context-sensitive task instructions  
- Toggles visibility  
- Uses `TaskPopupState` enum for updates  
- VR-friendly UI placement  

**Key Method**:  
- `setTaskTextByState()`: Updates instructions  

#### UserDataManager
- Tracks and serializes player data (JSON)  
- Records wrong answers in surveys  
- Saves data across sessions  

**Key Classes**:  
- `LevelSurveyData`, `SerializableLevelEntry`, `SerializableLevelsData`  

**Key Methods**:  
- `saveUserData()`  
- `addWrongAnswerToLevelSurvey()`  

#### SurveyManager
- Handles post-level questionnaires  
- Multiple choice validation and feedback  
- Unlocks levels based on performance  

---

### Interactive Components

#### SolverButtons
- Implements solutions with correct SVM setup  
- Adjusts decision planes per level  
- Triggers level completion  

**Key Method**:  
- `OnUIButtonClicked()`: Level-specific logic  

#### CreateSVMButtonScript
- Reads dropdown input  
- Maps features and classes  
- Creates dynamic SVMs via `SVMManager`  

**Key Method**:  
- `OnUIButtonClicked()`  

#### DecisionPlaneControlsScript
- VR-friendly plane adjustment  
- Real-time feedback  
- Encourages interactive learning  

#### StartButtonScript
- Starts game from menu  
- Triggers `GameManager`  
- Begins arrow guidance  

---

## Table of Contents
- Data Science VR Game  
- Overview  
- Game Structure  
- Level Breakdown  
- Breast Cancer Dataset  
- Technical Implementation  
  - Core Architecture  
  - Key Components  
    - GameManager  
    - Mini-Managers  
    - Interactive Components  
- Data Management  
- VR Implementation  
- Future Development  
- Detailed Component Documentation  
  - GameManager  
  - Mini-Managers  
    - ArrowManager  
    - DoorManager  
    - TaskPopupManager  
    - UserDataManager  
    - SurveyManager  
  - Interactive Components  
    - SolverButtons  
    - CreateSVMButtonScript  
    - DecisionPlaneControlsScript  
    - StartButtonScript  
