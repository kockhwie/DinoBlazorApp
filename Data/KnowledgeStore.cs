namespace DinoAI.Data;

public static class KnowledgeStore
{
    public static IReadOnlyList<EvolutionAge> Ages { get; } =
    [
        new("dinosaur-era",    "Dinosaur Era",    "Curiosity & Discovery",    "ti ti-egg-cracked",        "#33d7a6", 1),
        new("stone-age",       "Stone Age",       "Survival & Instinct",      "ti ti-bow",                "#d97706", 2),
        new("bronze-age",      "Bronze Age",      "Tools & Discovery",        "ti ti-shovel-pitchforks",  "#b45309", 3),
        new("industrial-age",  "Industrial Age",  "Machines & Efficiency",    "ti ti-settings-spark",     "#64748b", 4),
        new("information-age", "Information Age", "Data & Connectivity",      "ti ti-network",            "#0284c7", 5),
        new("digital-age",     "Digital Age",     "Computing & Automation",   "ti ti-cpu",                "#2563eb", 6),
        new("ai-era",          "AI Era",          "Intelligence & Evolution", "ti ti-brain",              "#3b82f6", 7),
    ];

    public static IReadOnlyList<KnowledgeCard> Cards { get; } =
    [
        // ── DINOSAUR ERA ──────────────────────────────────────────────────────────
        new(
            Slug:       "what-is-data",
            Title:      "What is Data?",
            Summary:    "At its core, data is simply raw information. Before computers, humans recorded data as tallies, words, and drawings.",
            WhyCare:    "Every app, report, dashboard, and AI answer starts with data.",
            Example:    "A spreadsheet of sales numbers, a photo on your phone, or a log of website visits — all of it is data.",
            WithoutIt:  "Software would have nothing to process and AI would have nothing to learn from.",
            Body:       "Data is any set of values, characters, or symbols that represents facts, observations, or instructions. On a computer, all data — whether a photo, a text message, or a high-definition video — is converted into basic numbers. Understanding data is the absolute starting point of all technology, because software and AI exist solely to process, analyze, and transform data into useful knowledge.",
            Category:   "Data & Logic",
            AgeKey:     "dinosaur-era",
            Icon:       "ti ti-database-heart",
            Tags:       "data,fundamentals,basics",
            SortOrder:  1
        ),
        new(
            Slug:       "what-is-binary",
            Title:      "What is Binary?",
            Summary:    "Binary is the language of 1s and 0s that powers every digital screen and chip in the world.",
            WhyCare:    "It explains how digital machines turn simple signals into useful work.",
            Example:    "The letter 'A' is stored as 01000001 — eight on/off switches that every device agrees on.",
            WithoutIt:  "Digital hardware would lose its simplest, most reliable way to represent information.",
            Body:       "Binary is a base-2 numeral system. While humans count using ten digits (0–9), digital circuits use only two states: ON (1) or OFF (0). These are called bits. By combining multiple bits, computers can represent letters, numbers, colors, and instructions. Understanding binary helps you realize that at the lowest level, all complex software and AI models are executing billions of simple on/off switches.",
            Category:   "Data & Logic",
            AgeKey:     "dinosaur-era",
            Icon:       "ti ti-binary",
            Tags:       "binary,computer-science,bits",
            SortOrder:  2
        ),
        new(
            Slug:       "what-is-an-algorithm",
            Title:      "What is an Algorithm?",
            Summary:    "An algorithm is a step-by-step recipe or set of rules to solve a specific problem.",
            WhyCare:    "Algorithms are the repeatable steps behind search, recommendations, automation, and AI.",
            Example:    "Google Search ranks results using algorithms that weigh hundreds of factors in milliseconds.",
            WithoutIt:  "Computers would not have repeatable instructions for solving problems.",
            Body:       "In computer science, an algorithm is a clear sequence of instructions designed to perform a task or solve a problem. Think of it like a recipe: you start with ingredients (input data), follow a series of precise steps (processing), and produce a meal (output). Every app, game, search engine, and AI model is built from algorithms. Some are simple (like sorting a list alphabetically), while others are incredibly complex (like deciding what to show on your social media feed).",
            Category:   "Data & Logic",
            AgeKey:     "dinosaur-era",
            Icon:       "ti ti-route",
            Tags:       "algorithm,logic,basics",
            SortOrder:  3
        ),

        // ── STONE AGE ─────────────────────────────────────────────────────────────
        new(
            Slug:       "the-internet",
            Title:      "What is the Internet?",
            Summary:    "The internet is the physical global network connecting billions of computers together.",
            WhyCare:    "Almost every modern service depends on connected devices exchanging data.",
            Example:    "Video calls, online banking, cloud apps, and AI chat all travel through the internet.",
            WithoutIt:  "Modern online services, cloud apps, and remote AI systems would not exist.",
            Body:       "The internet is a massive, global network of physical cables, wireless connections, routers, and switches. It allows computers all over the world to talk to each other. When you send a message, it is broken down into tiny 'packets' of data, routed through various paths across the globe in milliseconds, and reassembled on the receiving device. It is the foundation that enables the Web, cloud hosting, and cloud-based AI systems to exist.",
            Category:   "Networking",
            AgeKey:     "stone-age",
            Icon:       "ti ti-world",
            Tags:       "internet,networks,infrastructure",
            SortOrder:  1
        ),
        new(
            Slug:       "what-is-code",
            Title:      "What is Programming Code?",
            Summary:    "Code is how humans write instructions that computers can understand and execute.",
            WhyCare:    "Code is how teams turn ideas, business rules, and workflows into software.",
            Example:    "This very website was built with C# and Blazor — both are programming languages turned into running software.",
            WithoutIt:  "Most digital products would stay as ideas instead of becoming working tools.",
            Body:       "Computers only understand binary, which is too tedious for humans to write directly. Instead, we use programming languages (like C#, Python, JavaScript, and HTML) to write 'code'. A special program called a compiler or interpreter then translates this code into machine language (binary) that the computer's CPU can run. Writing code is like writing a very precise rulebook, telling the computer exactly how to react to clicks, input, and data.",
            Category:   "Software",
            AgeKey:     "stone-age",
            Icon:       "ti ti-code",
            Tags:       "code,programming,development",
            SortOrder:  2
        ),
        new(
            Slug:       "the-world-wide-web",
            Title:      "What is a Website?",
            Summary:    "A website is a collection of connected pages people can visit through a browser.",
            WhyCare:    "Websites are still the front door for most products, companies, and learning experiences.",
            Example:    "A company website, a product dashboard, or an online course is delivered through the Web.",
            WithoutIt:  "People would need separate installed software for many things they now open in a browser.",
            Body:       "While often confused with the internet, the World Wide Web is actually a service built on top of the internet. The internet is the physical connection, whereas the Web is the collection of documents, images, and pages linked by hyperlinks (URLs). We access the Web using a browser (like Chrome, Safari, or Edge) which downloads files using protocols like HTTP and renders them as interactive websites.",
            Category:   "Networking",
            AgeKey:     "stone-age",
            Icon:       "ti ti-browser",
            Tags:       "website,web,http,browser",
            SortOrder:  3
        ),

        // ── BRONZE AGE ────────────────────────────────────────────────────────────
        new(
            Slug:       "databases",
            Title:      "What is a Database?",
            Summary:    "A database is an organized, secure digital filing cabinet for storing structured data.",
            WhyCare:    "Without reliable storage, apps cannot remember users, orders, content, or decisions.",
            Example:    "A shopping cart remembers items because a database stores them.",
            WithoutIt:  "Apps would forget accounts, orders, inventory, messages, and settings.",
            Body:       "Unlike simple text files, databases are designed to store, retrieve, and update huge amounts of data securely and instantly. Relational databases (like PostgreSQL and SQL Server) organize data into structured tables with rows and columns, allowing powerful queries to find relationships. Without databases, websites couldn't store user accounts, inventories, or app settings.",
            Category:   "Data & Logic",
            AgeKey:     "bronze-age",
            Icon:       "ti ti-database",
            Tags:       "database,sql,storage",
            SortOrder:  1
        ),
        new(
            Slug:       "servers-and-clients",
            Title:      "Servers vs. Clients",
            Summary:    "The core architecture of the web: clients ask for resources, and servers provide them.",
            WhyCare:    "This pattern explains what happens whenever a browser, app, or AI tool asks for something.",
            Example:    "When you open this page, your browser (client) asked our server to send the content you are reading now.",
            WithoutIt:  "Browsers and apps would not have a clear way to request and receive services.",
            Body:       "A Client is the device or app you use (like your web browser or phone app) to request information. A Server is a powerful computer located somewhere in the world that listens for those requests and sends back the requested resources. When you query an AI, your browser is the client and the AI provider's machine is the server. This model defines how nearly all software on the internet communicates.",
            Category:   "Networking",
            AgeKey:     "bronze-age",
            Icon:       "ti ti-server",
            Tags:       "server,client,architecture",
            SortOrder:  2
        ),
        new(
            Slug:       "what-is-an-api",
            Title:      "What is an API?",
            Summary:    "An API (Application Programming Interface) is a messenger that lets two different software applications talk to each other.",
            WhyCare:    "APIs let products reuse maps, payments, AI models, and business systems instead of rebuilding everything.",
            Example:    "A food delivery app uses APIs for maps, payments, messages, and restaurant menus.",
            WithoutIt:  "Software systems would struggle to share data or reuse external capabilities.",
            Body:       "Think of an API like a waiter in a restaurant. You (the client app) look at the menu and place an order. The waiter (the API) takes your request to the kitchen (the server) and brings the result back. APIs allow developers to use existing services — like Google Maps, payment gateways, or AI language models — inside their own apps without rebuilding them from scratch.",
            Category:   "Software",
            AgeKey:     "bronze-age",
            Icon:       "ti ti-api",
            Tags:       "api,integration,development",
            SortOrder:  3
        ),

        // ── INDUSTRIAL AGE ────────────────────────────────────────────────────────
        new(
            Slug:       "automation",
            Title:      "What is Automation?",
            Summary:    "Automation is configuring software to perform repetitive tasks automatically without human intervention.",
            WhyCare:    "Automation removes repetitive work so people can focus on judgment and creativity.",
            Example:    "A script that emails a weekly sales report every Monday morning — without anyone pressing send.",
            WithoutIt:  "Humans would repeat manual steps that machines can perform consistently.",
            Body:       "In computing, automation means writing scripts or using tools to handle jobs like backing up files, sending weekly reports, or testing code. By automating mundane, repetitive tasks, humans can focus on creative problem-solving. Modern AI takes automation a step further, enabling machines to make complex decisions rather than just following rigid pre-programmed rules.",
            Category:   "Software",
            AgeKey:     "industrial-age",
            Icon:       "ti ti-robot",
            Tags:       "automation,efficiency,scripts",
            SortOrder:  1
        ),
        new(
            Slug:       "git-and-open-source",
            Title:      "Git & Open Source",
            Summary:    "Git tracks code history, while Open Source allows developers to collaborate globally on free software.",
            WhyCare:    "Most professional software work depends on shared code, history, and collaboration.",
            Example:    "GitHub hosts millions of open-source projects — including frameworks this site is built on.",
            WithoutIt:  "Teams would lose safer collaboration and much of the shared software ecosystem.",
            Body:       "Git is a version control system that lets developers track changes in code files, collaborate with others, and revert to older versions if something breaks. Open Source refers to software whose code is publicly available for anyone to inspect, modify, and share. Together they have accelerated global technology, allowing developers to build on top of community-driven software like Linux, Docker, and Blazor.",
            Category:   "Software",
            AgeKey:     "industrial-age",
            Icon:       "ti ti-git-fork",
            Tags:       "git,open-source,collaboration",
            SortOrder:  2
        ),

        // ── INFORMATION AGE ───────────────────────────────────────────────────────
        new(
            Slug:       "the-cloud",
            Title:      "What is the Cloud?",
            Summary:    "The Cloud means running software and storing data on someone else's internet-connected servers.",
            WhyCare:    "Cloud platforms let teams launch and scale software without buying all the hardware first.",
            Example:    "Microsoft 365 and Google Drive store files on remote servers you access anywhere.",
            WithoutIt:  "Companies would need far more physical hardware before launching or scaling products.",
            Body:       "Instead of keeping files and running applications on your own local computer or office server, the cloud allows you to rent computing power and storage from massive providers (like AWS, Azure, or Google Cloud). This makes services highly scalable, accessible from anywhere, and protected from local hardware failures. This very site runs on cloud infrastructure.",
            Category:   "Infrastructure",
            AgeKey:     "information-age",
            Icon:       "ti ti-cloud",
            Tags:       "cloud,servers,hosting",
            SortOrder:  1
        ),
        new(
            Slug:       "networks-and-ips",
            Title:      "Networks & IP Addresses",
            Summary:    "An IP address is a unique digital mailing address assigned to every device on a network.",
            WhyCare:    "Networking knowledge helps you understand how devices, services, and cloud systems find each other.",
            Example:    "Your home router has an IP address — that is how websites know where to send the page you requested.",
            WithoutIt:  "Devices and services would not know where to send information.",
            Body:       "For computers to send packets of data to each other, they need a way to identify destinations. An IP (Internet Protocol) address is a unique string of numbers (e.g. 192.168.1.1) assigned to your computer, phone, or router. Routers read this address to send your requests exactly where they need to go, similar to how mail carriers read zip codes.",
            Category:   "Networking",
            AgeKey:     "information-age",
            Icon:       "ti ti-network",
            Tags:       "ip-address,networking,routing",
            SortOrder:  2
        ),

        // ── DIGITAL AGE ───────────────────────────────────────────────────────────
        new(
            Slug:       "what-is-ai",
            Title:      "What is Artificial Intelligence?",
            Summary:    "AI is the science of making computers mimic human cognitive functions like learning and reasoning.",
            WhyCare:    "AI is changing how people write, analyze, build, support customers, and make decisions.",
            Example:    "ChatGPT, Copilot, support chatbots, and image recognition tools are everyday AI examples.",
            WithoutIt:  "Many modern assistants, recommendations, and automation tools would disappear.",
            Body:       "Artificial Intelligence (AI) is a broad field of computer science focused on building smart systems capable of performing tasks that typically require human intelligence. This includes understanding natural language, recognizing patterns in images, making decisions, and playing complex games. AI can be rule-based, but modern breakthroughs are powered by systems that learn from data.",
            Category:   "Artificial Intelligence",
            AgeKey:     "digital-age",
            Icon:       "ti ti-bulb",
            Tags:       "ai,foundations,basics",
            SortOrder:  1
        ),
        new(
            Slug:       "what-is-machine-learning",
            Title:      "What is Machine Learning?",
            Summary:    "Machine Learning is a subset of AI where computers learn patterns from data without being explicitly programmed.",
            WhyCare:    "Machine learning powers predictions, recommendations, ranking, and many AI systems.",
            Example:    "Netflix recommends shows based on what you and similar users watched — that is ML at work.",
            WithoutIt:  "Software would rely more on hand-written rules and less on patterns learned from data.",
            Body:       "In traditional programming, humans write rules to process inputs and get outputs. In Machine Learning (ML), we feed the computer lots of inputs and outputs (data), and the computer figures out its own mathematical rules (a model) to map the two. The model can then predict outputs for new, unseen inputs. Over time, the model learns and improves its accuracy by seeing more data.",
            Category:   "Artificial Intelligence",
            AgeKey:     "digital-age",
            Icon:       "ti ti-school",
            Tags:       "ml,algorithms,learning",
            SortOrder:  2
        ),
        new(
            Slug:       "what-is-deep-learning",
            Title:      "What is Deep Learning?",
            Summary:    "Deep Learning uses multilayered artificial neural networks to solve highly complex tasks.",
            WhyCare:    "Deep learning is behind many breakthroughs in language, vision, voice, and modern generative AI.",
            Example:    "Face unlock on your phone and real-time speech transcription both run on deep learning models.",
            WithoutIt:  "AI would struggle with complex language, vision, and speech tasks.",
            Body:       "Deep Learning is a specialized branch of Machine Learning inspired by the structure of the human brain. By stacking many layers of processing units (neural networks), deep learning algorithms automatically learn and extract complex features from raw data. This is what powers modern self-driving cars, face recognition, voice assistants, and the large language models behind AI chat tools.",
            Category:   "Artificial Intelligence",
            AgeKey:     "digital-age",
            Icon:       "ti ti-layers-difference",
            Tags:       "deep-learning,neural-networks,ai",
            SortOrder:  3
        ),

        // ── AI ERA ────────────────────────────────────────────────────────────────
        new(
            Slug:       "what-is-an-llm",
            Title:      "What is an LLM?",
            Summary:    "Large Language Models (LLMs) are AI engines trained on massive text data to predict and generate human-like language.",
            WhyCare:    "LLMs power the chat, writing, coding, and reasoning experiences people now expect from AI.",
            Example:    "When you ask ChatGPT to write an email, a large language model is predicting the best words to follow your prompt.",
            WithoutIt:  "AI would be much weaker at natural language conversations, writing, and code help.",
            Body:       "An LLM is a type of Deep Learning model designed to understand, process, and generate natural text. Trained on vast amounts of books, articles, and websites, an LLM learns the relationships between words. When you type a prompt, the model calculates the most likely sequence of words to follow, generating essays, answers, or programming code in real time.",
            Category:   "Artificial Intelligence",
            AgeKey:     "ai-era",
            Icon:       "ti ti-message-chatbot",
            Tags:       "llm,gpt,gemini",
            SortOrder:  1
        ),
        new(
            Slug:       "what-is-a-prompt",
            Title:      "What is a Prompt?",
            Summary:    "A prompt is the natural language instruction or query you give to an AI model to guide its response.",
            WhyCare:    "Prompts are the practical interface between human intent and AI output.",
            Example:    "\"Summarize this article in three bullet points for a non-technical manager\" is a well-structured prompt.",
            WithoutIt:  "People would have a harder time steering AI toward useful outcomes.",
            Body:       "Unlike traditional software, which requires strict commands or clicks, generative AI is guided by prompts. Writing an effective prompt — known as Prompt Engineering — involves giving the AI context, clear instructions, examples, and constraints. Good prompts unlock the model's capabilities and reduce incorrect or hallucinated answers. It is the core skill for anyone who wants to use AI productively.",
            Category:   "Prompt Engineering",
            AgeKey:     "ai-era",
            Icon:       "ti ti-sparkles",
            Tags:       "prompting,instructions,interaction",
            SortOrder:  2
        ),
        new(
            Slug:       "what-is-rag",
            Title:      "What is RAG?",
            Summary:    "Retrieval-Augmented Generation (RAG) is a technique that feeds custom documents to an LLM to give it up-to-date, accurate facts.",
            WhyCare:    "RAG helps AI answer from trusted company or product knowledge instead of guessing.",
            Example:    "An HR chatbot can answer questions about your company's leave policy by reading the actual policy PDF via RAG.",
            WithoutIt:  "AI would rely more on training data and be less grounded in current private knowledge.",
            Body:       "LLMs are only as knowledgeable as the data they were trained on. RAG solves this limit by connecting the LLM to an external data source (like a database or company PDFs). When a user asks a question, the system searches the database for relevant files, feeds those files to the LLM as background context, and asks the LLM to generate an answer based only on that context. This reduces errors and keeps private data secure.",
            Category:   "AI Architecture",
            AgeKey:     "ai-era",
            Icon:       "ti ti-database-import",
            Tags:       "rag,vector-search,architecture",
            SortOrder:  3
        ),
        new(
            Slug:       "ai-agents",
            Title:      "What are AI Agents?",
            Summary:    "AI agents are autonomous systems that can use tools, plan steps, and execute complex workflows without constant human supervision.",
            WhyCare:    "Agents move AI from answering questions to completing multi-step work.",
            Example:    "An agent can research a topic online, draft a report, and email it — all from a single instruction.",
            WithoutIt:  "People would need to manually perform every step across tools and workflows.",
            Body:       "While regular chatbots only respond to your text, AI Agents are designed to achieve goals. They can plan a series of tasks, decide which external tools to use (like searching the web, calling APIs, or running code), review their own output for mistakes, and iterate until the goal is achieved. They represent the next step of the AI Era — shifting from assistants to independent operators.",
            Category:   "AI Architecture",
            AgeKey:     "ai-era",
            Icon:       "ti ti-steering-wheel",
            Tags:       "agents,autonomy,tools",
            SortOrder:  4,
            ShortTitle: "AI Agents"
        ),
        new(
            Slug:       "what-is-mcp",
            Title:      "What is MCP?",
            Summary:    "MCP is a common way for AI systems to connect with tools, apps, and data sources.",
            WhyCare:    "MCP makes it easier for agents to reach the tools and data they need.",
            Example:    "An AI assistant can read your calendar, update a task board, and send a Slack message — all through MCP connectors.",
            WithoutIt:  "Every AI-tool connection would need more custom integration work.",
            Body:       "Model Context Protocol (MCP) gives AI applications a standard pattern for reaching external systems such as files, databases, business tools, and developer workflows. Instead of building a different custom integration for every tool, teams can expose capabilities in a consistent way and let AI agents use them more reliably. Think of it as a universal plug socket for AI tools.",
            Category:   "AI Architecture",
            AgeKey:     "ai-era",
            Icon:       "ti ti-plug-connected",
            Tags:       "mcp,agents,tools,integration",
            SortOrder:  5
        ),
    ];
}