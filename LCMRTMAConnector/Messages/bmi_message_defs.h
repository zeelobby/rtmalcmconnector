
#ifndef BMI_MESSAGE_DEFS_H // Added for C++ compiling
#define BMI_MESSAGE_DEFS_H

// Parameters
#define MMHOST				"192.168.0.1:7111" // ip_address:port of message manager server
#define N_LIMBS				2					// # of MPLs
#define N_JOINTS_PER_LIMB	27					// total number of joints in a limb
#define N_EP_DOF			7					// # MPL degrees of freedom: x, y, z, Roll, Pitch, Yaw, grasp position
#define N_NEUROPORTS		3		// # of neuroport devices
#define N_CHANNELS			128		// # of channels per neuroport
#define N_UNITS				6		// # of neuroport units per channel
#define N_BUFFER_FRAMES		1		// # of spike count bins buffered by neuroport interface module
#define N_DECODER_OUTPUTS	N_EP_DOF*N_LIMBS			// # of outputs from the neural decoder module, originally was 7
#define N_GAIN_BIAS_TERMS	N_DECODER_OUTPUTS*2	// # should be 2 times the numbers of of outputs from the neural decoder module
#define MAX_STIM_ELECTRODES 12		// # of electrodes to stimulate on the CereStim
#define MAX_STIM_CONFIGS	15		// # of stimulation configs that can be set
#define ERROR_STRING_LENGTH	256		// # of characters in the error strings on ack messages

// Module IDs
#define MID_NEUROPORT_INTERFACE				21 // start of Neuroport streamer nodes
#define MID_NEURAL_DECODER					22
#define MID_STIM_ENCODER					23 // start of CereStim nodes
#define MID_SAFETY_INTERFACE				24
#define MID_MPL_CTRL						25 // start of MPL control	
#define MID_VISUALIZER						26 // for viewing decoder output	
#define MID_COORD							27 // start of Coordinator
#define MID_PARADIGM_CTRL					28 // start of Paradigm Controller
#define MID_JOYSTICK_MODULE					29 // joystick module for substituting for the Neural Decoder
#define MID_COORD_2 						30 // Second module for Coordinator
#define MID_TWOAFC	 						31 // 2AFC 
#define MID_COORDINATOR_CLIENT_1            41
#define MID_COORDINATOR_CLIENT_2            42
#define MID_COORDINATOR_CLIENT_3			43
#define MID_COORDINATOR_CLIENT_4			44
#define MID_COORDINATOR_CLIENT_5			45	
#define MID_COORDINATOR_CLIENT_6			46
#define MID_LCMRTMA_RECEIVER				50
#define MID_LCMRTMA_TRANSMITTER				51

// Message Tags
#define MT_SPIKE_COUNTS			 			210	// outputs from MID_NEUROPORT_INTERFACE
#define MT_STREAMER_UPDATE					212 // control and config flags for MID_NEUROPORT_INTERFACE
#define MT_NEURAL_DECODER_OUTPUT			220 // outputs from MID_NEURAL_DECODER
#define MT_MANUAL_DECODER					221 // manual gain and bias parameters applied to MID_NEURAL_DECODER
#define MT_NEURAL_DECODER_UPDATE			222 // message to MID_NEURAL_DECODER to update its status/streaming/configs
#define MT_MPL_GOAL_STATUS					250 // info on whether MPL DOF are near goal states, vector of flags (0 or 1) that reflect whether the MPLs are in a goal location [x y z roll pitch raw grasp], first right MPL, then left MPL
#define MT_STIM_ENCODER_OUTPUT				230 // outputs from MID_STIM_ENCODER
#define MT_STIM_LEVELS_CONFIG				231 // config command to the safety module
#define MT_STIM_ENABLE						232 // true/false as to whether the stim encoder sends commands to the safety module
#define MT_SAFETY_ACK						233 // acknowledgement of what actually gets stimulated
#define MT_STIM_LEVELS_CONFIG_ACK			234	// acknowledgement of configuration commands
#define MT_SYS_CONFIG						998 // system-wide config
#define MT_SYS_EXIT							999 // stop cue
#define MT_MPL_LOCK_STATE					281 // lock/unlock MPL
#define MT_MPL_PARAMS						282 // parameters for control of MPL
#define MT_PARADIGM_MESSAGE					283 // message to the initialized Paradigm module that causes it to either start or stop executing 
#define MT_PARADIGM_FINISHED				284 // message from executing Paradigm module that it has finished its paradigm 
#define MT_SERIAL_MESSAGES					285 // message that copies any serial digital words that were sent to the Neuroports to the RTMA logger  
#define MT_MPL_REZERO						286 // command the MPL to rezero itself
#define MT_MOD_CTRL                         270 // used by Coordinator to start/stop BiCNS modules
#define MT_MOD_STATUS                       271 // used by CoordinatorClients to report status of BiCNS module startup
#define MT_RIGHT_PERCEPT					251 // joint angles from VulcanX for right MPL
#define MT_LEFT_PERCEPT						252 // joint angles from VulcanX for right MPL
#define MT_ENDPOINT_POSITIONS		     	253 // calculated endpoint positions, including ROC position
#define MT_RIGHT_COMMANDED_VELOCITIES		254 // endpoint and ROC velocities sent to right MPL
#define MT_LEFT_COMMANDED_VELOCITIES		255 // endpoint and ROC velocities sent to left MPL
#define MT_CLIENT_SAVE_STATUS				272 // Coordinator Client is starting/done copying session files

// Message definitions
typedef struct {
	unsigned char filepath[261];			// base filepath for all saved session files
	unsigned int savemode;						// 0 = don't save, 1 = save to session folder, 2 = archive to central server
} MDF_SYS_CONFIG;

typedef struct {
	unsigned int current_time;	// Neuroport clock cycle when message was sent
	unsigned int window_size;	// spike count window
	unsigned int timestamp;		// Neuroport clock cycle when spike bins begin (should be at least window_size*N_BUFFER_FRAMES before current_time)
	unsigned int padding;		// unused; for data alignment
	unsigned char spikes[N_NEUROPORTS * N_CHANNELS * N_UNITS * N_BUFFER_FRAMES];	// flattened 2D array of spike counts [row0, row1, row2]
																		// where columns are neural sources, and rows
																		// are the buffering in time (last row is most recent)
} MDF_SPIKE_COUNTS;

typedef struct {
	unsigned char use_logger;			// true = enables logging of NEV files through Central
	unsigned char use_streamer;			// true = enables streaming data to RTMA network
	unsigned char start;				// true = starts streaming and/or NEV logs
	unsigned char stop;					// true = stops streaming and/or NEV logs
} MDF_STREAMER_UPDATE;

typedef struct {
	//MSG_HEADER header;
	double decoderoutput[N_DECODER_OUTPUTS];
	int timestamp;		// timestamp (Neuroport clock cycles) of data packet the resulted in these commands
} MDF_NEURAL_DECODER_OUTPUT;

typedef struct {
	//MSG_HEADER header;
	double manualparameters[N_GAIN_BIAS_TERMS];
} MDF_MANUAL_DECODER;

typedef struct {
	//MSG_HEADER header;
	unsigned char flag_streamingstart; // flag that cues start streaming the output
	unsigned char flag_streamingstop; // flag that cues stop streaming the output
	unsigned char flag_loadparameters; // flag that cues a new set of model values should be loaded
} MDF_NEURAL_DECODER_UPDATE;

typedef struct {
	//MSG_HEADER header;
	unsigned char goalflags[N_DECODER_OUTPUTS]; // vector of flags (0 or 1) that reflect whether the MPLs are in a goal location [x y z roll pitch yaw grasp], first right MPL, then left MPL 
} MDF_MPL_GOAL_STATUS;

typedef struct {
	//MSG_HEADER header;
	unsigned char locked[N_LIMBS]; // true = limb locked - won't move (should be default), 0 = limb unlocked for motion
} MDF_MPL_LOCK_STATE;

typedef struct {
	//MSG_HEADER header;
	double	neuralDecoderScale[N_DECODER_OUTPUTS];			// scaling factor to use for neural decoder input. Set to 1 if gain and bias are used in the neural decoder.	
	double	goalEpPositions[N_DECODER_OUTPUTS];				// xyzRPYg right then left limb
	double	epTolerances[N_DECODER_OUTPUTS];				// tolerance for xyzRPYg right then left limb
	double	computerAssistMix[N_DECODER_OUTPUTS];			// 0-1: 0 = no computer assist, 1 = full computer assist
	double	computerAssistScaleFactors[N_DECODER_OUTPUTS];	// scale factor for  xyzRPYg right then left limb
	int		rocTable[N_LIMBS];								// ROC table ID to use for grasp - default to cylindrical (5)
	unsigned char orthoimpedanceEnabled[N_LIMBS];			// true = ortho-impedence enabled, false = ortho-impedance disabled (default)
    double  jointStiffness[N_LIMBS*N_JOINTS_PER_LIMB];		// joint stiffness factor per joint, right then left limb
} MDF_MPL_PARAMS;

typedef struct {
	//MSG_HEADER header;
	int electrodesToStim; // # of electrodes actually to be stimmed
	int electrodeNum[MAX_STIM_ELECTRODES]; // electrode ID on each cerestim
	int stimulatorIDs[MAX_STIM_ELECTRODES]; // 0-# of CereStims
	int amplitudes[MAX_STIM_ELECTRODES]; // 0-100, int
} MDF_STIM_ENCODER_OUTPUT;

typedef struct {
	//MSG_HEADER header;
	int electrodesToStim; // # of electrodes actually to be stimmed
	int electrodeNum[MAX_STIM_ELECTRODES]; // electrode ID on each cerestim
	int stimulatorIDs[MAX_STIM_ELECTRODES]; // 0-# of CereStims
	int amplitudes[MAX_STIM_ELECTRODES]; // 0-100, int
	char errorText[ERROR_STRING_LENGTH];
} MDF_SAFETY_ACK;

typedef struct {
	//MSG_HEADER header;
	int stimLevels[MAX_STIM_CONFIGS]; // electrode ID on each cerestim
	int ampFloor; // below this intensity, stimulation will be zeroed
} MDF_STIM_LEVELS_CONFIG;

typedef struct {
	//MSG_HEADER header;
	int stimLevels[MAX_STIM_CONFIGS]; // electrode ID on each cerestim
	int ampFloor; // below this intensity, stimulation will be zeroed
	char errorText[ERROR_STRING_LENGTH];
} MDF_STIM_LEVELS_CONFIG_ACK;

typedef struct {
	//MSG_HEADER header;
	unsigned char enableStim;	
} MDF_STIM_ENABLE;

typedef struct {
	unsigned char flag_streamingstart; // flag that cues start streaming the output
	unsigned char flag_streamingstop; // flag that cues stop streaming the output
	unsigned char flag_endtrial; // flag that allows manually ending of a paradigm trial 
	unsigned char flag_starttrial; // flag that allows manually starting an individual paradigm trial
} MDF_PARADIGM_MESSAGE;

typedef struct {
	int dummy;		// dummy variable
} MDF_PARADIGM_FINISHED;

typedef struct {
	//MSG_HEADER header;
	double serial;				// has the same number as is sent to the Neuroports for serial digital word logging, here it is a double, although only values of 0->255 will be used 
	double serialencode;		// when a fancy serial encoding is used for the Neuroports (required due to the 8-bit limit), for RTMA just send the value here as a double
} MDF_SERIAL_MESSAGES;

typedef struct {
	//MSG_HEADER header;
	unsigned char dummy; // unused - the fact that it appears triggers action
} MDF_MPL_REZERO;

typedef struct {
	//MSG_HEADER header;
	int module;					// ID of module to stop. 0 = all modules that respond to the message
} MDF_SYS_EXIT;

typedef struct {
	//MSG_HEADER header;
	int module;	// ID of module to stop. 0 = all modules that respond to the message
	int action; //0=stop, 1=start

} MDF_MOD_CTRL;

typedef struct {
	//MSG_HEADER header;
	int module; // ID of module to stop. 0 = all modules that respond to the message
    int status; // 0=stopped, 1=running
} MDF_MOD_STATUS;

typedef struct {
	//MSG_HEADER header;
	double jointAngles[27];
} MDF_RIGHT_PERCEPT;

typedef struct {
	//MSG_HEADER header;
	double jointAngles[27];
} MDF_LEFT_PERCEPT;

typedef struct {
	//MSG_HEADER header;
	double endpointPosition[N_LIMBS*N_EP_DOF]; // x,y,z,R,P,Y,ROC pos for right then left limb
} MDF_ENDPOINT_POSITIONS;

typedef struct {
	//MSG_HEADER header;
	double velocity[N_EP_DOF]; // commanded x,y,z,R,P,Y,ROC velocities
	double rocTable;
	double impedance[N_JOINTS_PER_LIMB]; // impedances for all joints
} MDF_RIGHT_COMMANDED_VELOCITIES; 

typedef struct {
	//MSG_HEADER header;
	double velocity[N_EP_DOF]; // commanded x,y,z,R,P,Y,ROC velocities + impedances for all joints
	double rocTable;
	double impedance[N_JOINTS_PER_LIMB]; // impedances for all joints
} MDF_LEFT_COMMANDED_VELOCITIES;

typedef struct {
	//MSG_HEADER header;
	int clientID; // ID of the client (use it's MM Module ID)
	int status;   // 0 = starting to save, 1 = done saving
} MDF_CLIENT_SAVE_STATUS;

#endif