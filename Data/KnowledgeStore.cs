using System;
using System.Collections.Generic;

namespace DinoAI.Data;

public static class KnowledgeStore
{
    public static IReadOnlyList<EvolutionAge> Ages { get; } = new List<EvolutionAge>
    {
        new("stone-age", "Stone Age", "Survival & Instinct", "ti ti-egg-cracked", "#d97706", 1), // amber-600
        new("bronze-age", "Bronze Age", "Tools & Discovery", "ti ti-shovel-pitchforks", "#b45309", 2), // amber-700
        new("iron-age", "Iron Age", "Strength & Advancement", "ti ti-swords", "#78350f", 3), // amber-900
        new("industrial-age", "Industrial Age", "Machines & Efficiency", "ti ti-settings-spark", "#64748b", 4), // slate-500
        new("information-age", "Information Age", "Data & Connectivity", "ti ti-network", "#0284c7", 5), // sky-600
        new("digital-age", "Digital Age", "Computing & Automation", "ti ti-cpu", "#2563eb", 6), // blue-600
        new("ai-era", "AI Era", "Intelligence & Evolution", "ti ti-brain", "#3b82f6", 7) // blue-500
    };

    public static IReadOnlyList<KnowledgeCard> Cards { get; } = new List<KnowledgeCard>
    {
        // ── STONE AGE ─────────────────────────────────────────────────────────────
        new(
            "what-is-data",
            "What is Data?",
            "At its core, data is simply raw information. Before computers, humans recorded data as tallies, words, and drawings.",
            "Data is any set of values, characters, or symbols that represents facts, observations, or instructions. On a computer, all data—whether a photo, a text message, or a high-definition video—is converted into basic numbers. Understanding data is the absolute starting point of all technology, because software and AI exist solely to process, analyze, and transform data into useful knowledge.",
            "Data & Logic",
            "stone-age",
            "ti ti-database-heart",
            "data,fundamentals,basics",
            1
        ),
        new(
            "what-is-binary",
            "What is Binary?",
            "Binary is the language of 1s and 0s that powers every digital screen and chip in the world.",
            "Binary is a base-2 numeral system. While humans count using ten digits (0-9), digital circuits use only two states: ON (represented by 1) or OFF (represented by 0). These are called bits. By combining multiple bits together, computers can represent letters, numbers, colors, and instructions. For example, the letter 'A' is represented in binary as `01000001`. Understanding binary helps you realize that at the lowest level, all complex software and AI models are executing billions of simple on-off switches.",
            "Data & Logic",
            "stone-age",
            "ti ti-binary",
            "binary,computer-science,bits",
            2
        ),
        new(
            "what-is-an-algorithm",
            "What is an Algorithm?",
            "An algorithm is a step-by-step recipe or set of rules to solve a specific problem.",
            "In computer science, an algorithm is a clear sequence of instructions designed to perform a task or solve a problem. Think of it like a recipe: you start with ingredients (input data), follow a series of precise steps (processing), and produce a meal (output). Every app, game, search engine, and AI model is built from algorithms. Some are simple (like sorting a list alphabetically), while others are incredibly complex (like deciding what to show on your social media feed).",
            "Data & Logic",
            "stone-age",
            "ti ti-route",
            "algorithm,logic,basics",
            3
        ),

        // ── BRONZE AGE ────────────────────────────────────────────────────────────
        new(
            "the-internet",
            "What is the Internet?",
            "The internet is the physical global network connecting billions of computers together.",
            "The internet is a massive, global network of physical cables, wireless connections, routers, and switches. It allows computers all over the world to talk to each other. When you send a message, it is broken down into tiny 'packets' of data, routed through various paths across the globe in milliseconds, and reassembled on the receiving device. It is the foundation that enables the Web, cloud hosting, and cloud-based AI systems to exist.",
            "Networking",
            "bronze-age",
            "ti ti-world",
            "internet,networks,infrastructure",
            1
        ),
        new(
            "what-is-code",
            "What is Programming Code?",
            "Code is how humans write instructions that computers can understand and execute.",
            "Computers only understand binary, which is too tedious for humans to write directly. Instead, we use programming languages (like C#, Python, JavaScript, and HTML) to write 'code'. A special program called a compiler or interpreter then translates this code into machine language (binary) that the computer's CPU can run. Writing code is like writing a very precise rulebook, telling the computer exactly how to react to clicks, input, and data.",
            "Software",
            "bronze-age",
            "ti ti-code",
            "code,programming,development",
            2
        ),
        new(
            "the-world-wide-web",
            "The World Wide Web (WWW)",
            "The Web is an information system of websites and pages built on top of the internet.",
            "While often confused with the internet, the World Wide Web is actually a service built *on top* of the internet. The internet is the physical connection, whereas the Web is the collection of documents, images, and pages linked by hyperlinks (URLs). We access the Web using a web browser (like Chrome, Safari, or Edge) which downloads files using protocols like HTTP and renders them on our screens as interactive websites.",
            "Networking",
            "bronze-age",
            "ti ti-browser",
            "web,http,browser",
            3
        ),

        // ── IRON AGE ──────────────────────────────────────────────────────────────
        new(
            "databases",
            "What is a Database?",
            "A database is an organized, secure digital filing cabinet for storing structured data.",
            "Unlike simple text files, databases are designed to store, retrieve, and update huge amounts of data securely and instantly. Relational databases (like SQL Server, PostgreSQL, and MySQL) organize data into structured tables with rows and columns, allowing you to run powerful queries to find relationships. Without databases, websites couldn't store user accounts, inventories, or app settings.",
            "Data & Logic",
            "iron-age",
            "ti ti-database",
            "database,sql,storage",
            1
        ),
        new(
            "servers-and-clients",
            "Servers vs. Clients",
            "The core architecture of the web: clients ask for resources, and servers provide them.",
            "In software development, this model defines how devices communicate. A **Client** is the device or app you use (like your web browser or phone app) to request information. A **Server** is a powerful computer located somewhere in the world that listens for those requests and sends back the requested resources (like a web page, file, or search result). When you query an AI, your browser is the client, and the AI host is the server.",
            "Networking",
            "iron-age",
            "ti ti-server",
            "server,client,architecture",
            2
        ),
        new(
            "what-is-an-api",
            "What is an API?",
            "An API (Application Programming Interface) is a messenger that lets two different software applications talk to each other.",
            "Think of an API like a waiter in a restaurant. You (the client app) look at the menu and place an order. The waiter (the API) takes your request to the kitchen (the server or database) and brings the food back to you. APIs allow developers to use existing services (like Google Maps, payment gateways, or OpenAI's language models) inside their own apps without rebuilding them from scratch.",
            "Software",
            "iron-age",
            "ti ti-api",
            "api,integration,development",
            3
        ),

        // ── INDUSTRIAL AGE ────────────────────────────────────────────────────────
        new(
            "automation",
            "What is Automation?",
            "Automation is configuring software to perform repetitive tasks automatically without human intervention.",
            "In computing, automation means writing scripts or using tools to handle jobs like backing up files, sending weekly reports, or testing code. By automating mundane, repetitive tasks, humans can focus on creative problem-solving. Modern AI takes automation a step further, enabling machines to make complex decisions rather than just following rigid pre-programmed rules.",
            "Software",
            "industrial-age",
            "ti ti-robot-off",
            "automation,efficiency,scripts",
            1
        ),
        new(
            "git-and-open-source",
            "Git & Open Source",
            "Git tracks code history, while Open Source allows developers to collaborate globally on free software.",
            "**Git** is a version control system that lets developers track changes in code files, collaborate with others, and revert to older versions if something breaks. **Open Source** refers to software whose code is publicly available for anyone to inspect, modify, and share. Together, they have accelerated global technology, allowing developers to build on top of community-driven software like Linux, Docker, and Blazor.",
            "Software",
            "industrial-age",
            "ti ti-git-fork",
            "git,open-source,collaboration",
            2
        ),

        // ── INFORMATION AGE ───────────────────────────────────────────────────────
        new(
            "the-cloud",
            "What is the Cloud?",
            "The Cloud means running software and storing data on someone else's internet-connected servers.",
            "Instead of keeping files and running applications on your own local computer or physical office server, 'the cloud' allows you to rent computing power and storage from massive tech providers (like Amazon Web Services, Microsoft Azure, or Google Cloud). This makes services highly scalable, accessible from anywhere in the world, and protected from local hardware failures.",
            "Infrastructure",
            "information-age",
            "ti ti-cloud",
            "cloud,servers,hosting",
            1
        ),
        new(
            "networks-and-ips",
            "Networks & IP Addresses",
            "An IP address is a unique digital mailing address assigned to every device on a network.",
            "For computers to send packets of data to each other, they need a way to identify destinations. An IP (Internet Protocol) address is a unique string of numbers (e.g., `192.168.1.1` or a longer IPv6 format) assigned to your computer, phone, or router. Routers read this address to send your requests exactly where they need to go, similar to how mail carriers read zip codes.",
            "Networking",
            "information-age",
            "ti ti-network",
            "ip-address,networking,routing",
            2
        ),

        // ── DIGITAL AGE ───────────────────────────────────────────────────────────
        new(
            "what-is-ai",
            "What is Artificial Intelligence?",
            "AI is the science of making computers mimic human cognitive functions like learning and reasoning.",
            "Artificial Intelligence (AI) is a broad field of computer science focused on building smart systems capable of performing tasks that typically require human intelligence. This includes understanding natural language, recognizing patterns in images, making decisions, and playing complex games. AI can be rule-based, but modern breakthroughs are powered by systems that learn from data.",
            "Artificial Intelligence",
            "digital-age",
            "ti ti-bulb",
            "ai,foundations,basics",
            1
        ),
        new(
            "what-is-machine-learning",
            "What is Machine Learning?",
            "Machine Learning is a subset of AI where computers learn patterns from data without being explicitly programmed.",
            "In traditional programming, humans write rules to process inputs and get outputs. In Machine Learning (ML), we feed the computer lots of inputs and outputs (data), and the computer writes its own mathematical rules (a model) to map the two. The model can then predict outputs for new, unseen inputs. Over time, the model 'learns' and improves its accuracy by reviewing more data.",
            "Artificial Intelligence",
            "digital-age",
            "ti ti-school",
            "ml,algorithms,learning",
            2
        ),
        new(
            "what-is-deep-learning",
            "What is Deep Learning?",
            "Deep Learning uses multilayered artificial neural networks to solve highly complex tasks.",
            "Deep Learning is a specialized branch of Machine Learning. It is inspired by the structure of the human brain. By stacking many layers of processing units (neural networks), deep learning algorithms can automatically learn and extract complex features from raw data. This is what powers modern self-driving cars, face recognition, and voice assistants.",
            "Artificial Intelligence",
            "digital-age",
            "ti ti-layers-difference",
            "deep-learning,neural-networks,ai",
            3
        ),

        // ── AI ERA ────────────────────────────────────────────────────────────────
        new(
            "what-is-an-llm",
            "What is an LLM?",
            "Large Language Models (LLMs) are AI engines trained on massive text data to predict and generate human-like language.",
            "An LLM is a type of Deep Learning model designed to understand, process, and generate natural text. Trained on vast amounts of books, articles, and websites, an LLM learns the relationships between words. When you type a prompt, the model calculates the most likely sequence of words to follow, generating essays, answers, or programming code in real-time.",
            "Artificial Intelligence",
            "ai-era",
            "ti ti-message-chatbot",
            "llm,gpt,gemini",
            1
        ),
        new(
            "what-is-a-prompt",
            "What is a Prompt?",
            "A prompt is the natural language instruction or query you give to an AI model to guide its response.",
            "Unlike traditional software, which requires strict commands or clicks, generative AI is guided by 'prompts'. Writing an effective prompt (known as Prompt Engineering) involves giving the AI context, instructions, examples, and constraints. Good prompts unlock the model's capabilities and reduce incorrect or hallucinated answers.",
            "Prompt Engineering",
            "ai-era",
            "ti ti-sparkles",
            "prompting,instructions,interaction",
            2
        ),
        new(
            "what-is-rag",
            "What is RAG?",
            "Retrieval-Augmented Generation (RAG) is a technique that feeds custom documents to an LLM to give it up-to-date, accurate facts.",
            "LLMs are only as knowledgeable as the data they were trained on. RAG solves this limit by connecting the LLM to an external data source (like a database or company PDFs). When a user asks a question, the system searches the database for relevant files, feeds those files to the LLM as background context, and asks the LLM to generate an answer based *only* on that context. This reduces errors and secures custom data.",
            "AI Architecture",
            "ai-era",
            "ti ti-database-import",
            "rag,vector-search,architecture",
            3
        ),
        new(
            "ai-agents",
            "What are AI Agents?",
            "AI agents are autonomous systems that can use tools, plan steps, and execute complex workflows without constant human supervision.",
            "While regular chatbots only respond to your text, AI Agents are designed to achieve goals. They can plan a series of tasks, decide which external tools to use (like searching the web, calling APIs, or running code), review their own output for mistakes, and iterate until the goal is achieved. They represent the next step of the AI Era—shifting from assistants to independent operators.",
            "AI Architecture",
            "ai-era",
            "ti ti-steering-wheel",
            "agents,autonomy,tools",
            4
        )
    };
}
