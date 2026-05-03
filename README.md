# Virtual-Chemistry-Lab-
## Virtual Reality Science Practical System (VRSPS)

**Virtual-Chemistry-Lab-** is a Unity-based Virtual Reality (VR) science practical platform designed to improve practical science learning in resource-limited schools. It provides immersive, interactive laboratory simulations aligned with the **Ugandan curriculum**, enabling learners to perform experiments safely, repeatedly, and with guided feedback.

---

## Table of contents

- [Project overview](#project-overview)
- [Problem statement](#Problems-addressed)
- [Project goals](#project-goals)
- [Key features](#key-features)
- [Educational impact](#educational-impact)
- [Curriculum alignment](#curriculum-alignment-uganda)
- [Technology stack](#technology-stack)
- [System requirements](#system-requirements)
- [Installation and setup](#installation-and-setup)
- [How to use](#how-to-use)
- [Project structure](#project-structure)
- [Assessment and learning analytics](#assessment-and-learning-analytics)
- [Limitations](#limitations)
- [Future improvements](#future-improvements)
- [Contributing](#contributing)
- [Acknowledgements](#acknowledgements)



## Project overview

VRSPS recreates a school laboratory experience in VR so students can:

- interact with realistic lab apparatus
- perform practical experiments step-by-step
- observe outcomes in real time
- learn proper lab safety procedures

The system is especially intended for schools facing challenges such as:

- limited access to laboratory infrastructure
- shortage of chemicals and equipment
- safety risks during practical lessons
- large student populations with limited lab time



## Problems addressed

Many schools struggle to provide consistent, high-quality practical science experiences due to cost and safety constraints. As a result, learners may understand theory but lack confidence and competence in hands-on science procedures.

**VRSPS addresses this gap** by offering a reusable and safe virtual lab where students can practice experiments multiple times before or alongside physical lab sessions.


## Project goals

1. Improve practical science understanding through immersive learning.
2. Increase laboratory safety awareness and behavior.
3. Support curriculum-aligned experiment delivery.
4. Enable repeatable practical training without consumable costs.
5. Provide equitable access to practical learning in under-resourced settings.



## Key features

- **Immersive 3D laboratory environment:** Students work in a realistic virtual science lab with familiar equipment and spatial interaction.
- **Interactive experiment workflow:** Learners perform procedures by selecting tools, measuring substances, mixing reagents, and observing reactions.
- **Safety-first simulation design:** The platform emphasizes correct PPE use, proper handling of apparatus, and hazard awareness.
- **Guided learning experience:** Instructions and prompts guide students through each practical step.
- **Replay and practice support:** Experiments can be repeated until students master the process.
- **VR-based engagement:** Natural movement and manipulation can improve focus, retention, and learner motivation.



## Combustion reaction safety simulation example

A **combustion reaction** has been integrated to represent and teach **laboratory safety in VR**.

### Why this matters

Combustion involves heat and flame hazards that are often risky in real school settings. Simulating it in VR allows students to:

- learn correct safety precautions before physical exposure
- understand hazard conditions and safe response steps
- observe consequences of unsafe behavior in a controlled environment

### Safety learning outcomes demonstrated in this simulation

- identifying flammable materials and ignition sources
- following proper setup and procedural order
- maintaining safe distance and controlled handling
- practicing emergency-aware behavior in practical sessions

This feature strengthens both conceptual understanding of combustion and practical safety competence.



## Educational impact

VRSPS is designed to produce measurable educational benefits:

- better practical readiness and confidence
- stronger understanding of scientific procedures
- improved safety compliance in laboratory behavior
- increased learner participation through immersive interaction

It can be used for:

- pre-lab orientation
- in-class practical reinforcement
- exam preparation and revision
- remedial practical support



## Curriculum alignment (Uganda)

The system is developed with the intention of aligning practical content with the **Ugandan science curriculum** by:

- matching experiment topics and objectives to classroom learning outcomes
- supporting practical competencies expected at secondary level
- reinforcing both scientific reasoning and safe laboratory practice



## Technology stack

- **Game engine:** Unity
- **XR framework:** OpenXR / Unity XR Interaction Toolkit
- **Language:** C#
- **Platform focus:** VR-enabled learning environments
- **3D assets made with Blender:** Laboratory scene models, apparatus, and effects integrated into Unity



## System requirements

**Recommended development setup**

- Unity Editor version (6000.1.1f1)
- Visual Studio or VS Code to write, debug, and manage the code that controls interactions and behavior in the application
- High Performance PC (8GB RAM or more)
- Compatible VR headset and controllers e.g Meta Quest 3
- OpenXR runtime configured



## Installation and setup

1. Clone the repository:

   ```bash
   git clone https://github.com/ssenyonjoalvin/Virtual-Chemistry-Lab-
   ```

2. Open the project in Unity Hub.
3. Install required Unity packages (XR/OpenXR dependencies if not auto-resolved).
4. Configure headset runtime and OpenXR settings.
5. Open the main laboratory scene (and experiment-specific scenes as applicable).
6. Press **Play** in Unity or build to your target VR platform.



## How to use

1. Launch the application through the **Unity Editor** or a **built VR build**.
2. Enter the **virtual laboratory environment**.
3. Follow **on-screen instructions or prompts** for the selected experiment.
4. Interact with lab equipment using **VR controllers**:
   - pick up apparatus
   - measure and combine substances
   - position items correctly
5. Perform the experiment **step-by-step** as guided.
6. Observe the **results and feedback** provided by the system.
7. **Repeat** the experiment if needed to reinforce understanding or correct mistakes.



## Project structure

The project follows a **modular Unity structure**:

```text
Virtual-Chemistry-Lab-/
├── Scenes/
│   ├── Main laboratory scene
│   └── Experiment-specific scenes (e.g., combustion simulation)
├── Scripts/
│   ├── Interaction logic (object handling, triggers)
│   ├── Experiment workflows and step validation
│   └── Safety feedback systems
├── Prefabs/
│   ├── Reusable lab equipment (Bunsen burner, test tubes, etc.)
│   └── UI components and instructional elements
├── Assets/
│   ├── 3D models (lab environment, apparatus)
│   ├── Materials, textures, and animations
│   └── Sound effects (if included)
└── XR Setup/
    ├── XR Interaction Toolkit configurations
    └── Controller mappings and input actions
And many more...............................
```


## Assessment and learning analytics

VRSPS incorporates **basic performance tracking** to support learning evaluation.

### Step completion tracking

Monitors whether students follow the correct experimental sequence.

### Error detection

Identifies incorrect actions (for example, unsafe handling or wrong procedure order).

### Feedback mechanism

Provides immediate corrective guidance during the experiment.

### Performance indicators

- task completion accuracy
- number of retries
- safety compliance behavior

These analytics can help educators assess:

- student readiness for real lab work
- understanding of procedures
- adherence to safety practices



## Limitations

### Hardware dependency

Requires VR-capable devices, which may not be widely available in all schools.

### Limited experiment coverage

Currently supports a small number of simulations (for example: combustion , titration).

### Learning curve

New users may need time to adapt to VR controls and interactions.

### Performance constraints

Lower-end systems may experience lag or reduced graphical quality.

### Lack of advanced analytics

Does not yet include detailed reporting dashboards for educators.



## Future improvements

### Expanded experiment library

Add more curriculum-aligned experiments (for example, electrolysis, Organic Chemistry etc.).

### Multi-user collaboration

Enable multiple students to interact in the same virtual lab session.

### Advanced analytics dashboard

Provide teachers with detailed performance reports and insights.

### Non-VR mode support

Allow access via desktop (keyboard and mouse) for schools without VR hardware.

### Enhanced realism

- improved physics simulation
- more detailed chemical reactions and visual effects

### Assessment integration

Include quizzes, grading systems and assessments.



## Contributing

Contributions are welcome. Suggested contribution areas:

- new practical simulations
- UX and accessibility improvements
- curriculum mapping refinement
- performance optimization for lower-end hardware



## Acknowledgements

-Dr. Mwotil Alex (Supervisor)
- Educators and institutions supporting practical science innovation
- Unity and XR community resources
- All testers and contributors who helped validate classroom usability
