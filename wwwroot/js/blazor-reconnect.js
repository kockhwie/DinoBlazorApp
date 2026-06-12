Blazor.start({
  circuit: {
    reconnectionOptions: {
      maxRetries: 30,
      retryIntervalMilliseconds: (previousAttempts, maxRetries) =>
        previousAttempts >= maxRetries
          ? null
          : previousAttempts < 5
            ? 2000
            : Math.min(previousAttempts * 1000, 15000),
    },
  },
});
