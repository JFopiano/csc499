# Machine Learning VR Game

## Overview

This Virtual Reality game, developed in Unity, provides an immersive educational experience focused on teaching Support Vector Machine (SVM) learning algorithms. Players progress through a school environment with various rooms (levels), solving machine learning challenges to advance.

## Game Structure

- Players begin in a school environment
- Progression through 4 levels (with level 5 in development)
- Each level contains SVM-related puzzles to solve
- Post-level questionnaires assess understanding
- Doors open to new levels upon successful completion

## Level Breakdown

1. **Level 1**: Introduction to basic SVM concepts
2. **Level 2**: SVM with outliers
3. **Level 3**: More complex decision boundaries
4. **Level 4**: Dynamic SVM creation using the Iris dataset
5. **Level 5**: In development

## Iris Dataset

The game uses the famous Iris flower dataset (Iris.csv) to dynamically create SVMs in Level 4. Players can select features and species to visualize different decision boundaries.

## Technical Implementation

### Core Architecture

The game follows a manager-based architecture with a central GameManager coordinating various specialized mini-managers.

### Key Components

#### GameManager (`src/manager/GameManager.cs`)

- Central hub managing game state and player progression
- Coordinates all mini-managers
- Initializes level-specific SVMs
- Tracks player progression through levels
- Handles VR controller interactions

#### SVMManager (`src/manager/mini-managers/SVMManager.cs`)

- Core ML implementation
- Creates and manages SVM instances for different levels
- Handles hyperplane visualization and support vector positioning
- Implements Level 4's dynamic SVM creation using the Iris dataset
- Provides normalization and simple SVM training functionality

#### Mini-Managers

- **ArrowManager**: Guides player movement with visual cues
- **DoorManager**: Controls door states for level progression
- **TaskPopupManager**: Manages in-game task instructions
- **UserDataManager**: Tracks player progress and survey responses
- **SurveyManager**: Handles assessment questionnaires

#### Interactive Components (`src/scripts/`)

- **SolverButtons**: Implements level-specific solution buttons
- **CreateSVMButtonScript**: Handles dynamic SVM creation in Level 4
- **DecisionPlaneControlsScript**: Controls for manipulating decision boundaries
- **StartButtonScript**: Handles game initialization

### Data Management

The game persists user data using JSON serialization, tracking progress and survey responses across sessions.

## VR Implementation

- Built using Unity's XR Interaction Toolkit
- Supports left-hand task hologram for convenient instruction viewing
- Implements intuitive VR interactions with SVMs and game elements

## Future Development

- Level 5 implementation (in progress)
- Additional ML algorithms
- Enhanced visualization techniques

## Detailed Component Documentation

### GameManager

The GameManager serves as the central orchestrator for the entire application, managing game state and coordinating all subsystems.

**Key Responsibilities:**

- Initializes and manages all mini-managers through the `initManagers()` method
- Tracks player progression with boolean flags for each level (entered, solved, correctly answered)
- Handles VR controller input via `CheckSecondaryButtonPressedOnLeftController()`
- Manages the task hologram attached to the player's left hand
- Controls player movement enablement via the GameControlManager
- Coordinates level transitions and arrow guidance systems

**Key Methods:**

- `startGame()`: Initiates gameplay, enables movement, and starts Level 1 arrow sequences
- `keepTryingToPutTaskHologramInPlayerLeftHand()`: Coroutine ensuring the task display follows the player
- `Start()`: Initializes SVMs for the first two levels and sets up the initial game state
- `Update()`: Monitors for controller input to toggle task popup visibility

### SVMManager

The SVMManager implements the core machine learning functionality, particularly focusing on SVM visualization and interaction.

**Key Components:**

- `SVMInstance` class: Represents individual SVM visualizations with hyperplanes and support vectors
- `SupportVectorPosition` class: Stores positional data for support vectors in the visualization
- `SupportVectorInteraction` class: Handles user interaction with support vectors in VR

**Key Features:**

- Predefined SVM setups for Levels 1-3 with appropriate data points
- Dynamic SVM creation for Level 4 using the Iris dataset
- Support for feature selection and species filtering in Level 4
- Simple SVM training implementation using gradient descent
- Data normalization to fit visualization space

**Key Methods:**

- `createLevel1SVM()`, `createLevel2SVM()`: Set up predefined SVMs with support vectors
- `createLevel4SVM()`: Creates dynamic SVMs based on selected Iris dataset features
- `SolveLevel4SVM()`: Implements the solving algorithm for Level 4
- `TrainSVM()`: Basic SVM training implementation using gradient descent
- `NormalizeValue()`: Handles data normalization for visualization

### Mini-Managers

#### ArrowManager

Manages guidance arrows that help players navigate through the VR environment.

**Key Features:**

- Sequential lighting of arrows to guide player progression
- Different arrow sequences for each level
- Visual feedback to indicate direction and next objectives

**Key Methods:**

- `startLevel1ArrowLightingSequence()` through `startLevel4ArrowLightingSequence()`: Initiates arrow guidance for respective levels
- `stopLevelNArrowLightingSequence()`: Terminates arrow sequences when objectives are completed

#### DoorManager

Controls the state of doors that gate progress between levels.

**Key Features:**

- Door locking/unlocking based on level completion and survey results
- Visual feedback for door states (locked/unlocked)
- Progression control through physical barriers

#### TaskPopupManager

Handles the display of task instructions and feedback to the player.

**Key Features:**

- Context-sensitive task text based on game state
- Toggle functionality for task visibility
- State-based text updates via `TaskPopupState` enum
- VR-friendly UI positioning

**Key Method:**

- `setTaskTextByState()`: Updates task text based on the current game state/level

#### UserDataManager

Manages persistent player data, particularly focusing on survey responses.

**Key Features:**

- JSON serialization of player progress data
- Tracking of wrong answer selections in surveys
- File I/O for data persistence across sessions

**Key Classes:**

- `LevelSurveyData`: Stores wrong answers and calculates attempts
- `SerializableLevelEntry` and `SerializableLevelsData`: Support JSON serialization

**Key Methods:**

- `saveUserData()`: Persists player data to disk
- `addWrongAnswerToLevelSurvey()`: Records incorrect survey responses

#### SurveyManager

Handles the questionnaires presented after each level.

**Key Features:**

- Multiple-choice questions related to SVM concepts
- Answer validation and feedback
- Progression control based on correct answers
- Data recording for player performance analysis

### Interactive Components

#### SolverButtons

Implements the solution buttons for each level that apply the correct SVM configuration.

**Key Features:**

- Level-specific solution implementations
- Transformation of decision planes to the correct orientation
- Level completion triggering and state updates
- Special handling for Level 3's deformable plane

**Key Method:**

- `OnUIButtonClicked()`: Contains level-specific solution logic triggered by button presses

#### CreateSVMButtonScript

Handles the dynamic creation of SVMs in Level 4 based on user selections.

**Key Features:**

- Reads dropdown selections for features and species
- Maps selection text to enum values
- Positions and configures the decision plane
- Invokes SVMManager to create the appropriate SVM visualization

**Key Method:**

- `OnUIButtonClicked()`: Processes user selections and creates a custom SVM

#### DecisionPlaneControlsScript

Provides interactive controls for manipulating decision boundaries.

**Key Features:**

- VR-friendly manipulation of hyperplane position and orientation
- Real-time feedback on classification changes
- Interactive learning through direct manipulation

#### StartButtonScript

Initializes game state when the player begins the experience.

**Key Features:**

- Triggers game initialization via GameManager
- Enables player movement and interaction
- Begins the first guidance arrow sequence
